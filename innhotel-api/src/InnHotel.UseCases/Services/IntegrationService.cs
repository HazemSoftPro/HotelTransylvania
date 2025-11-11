using InnHotel.Core.Integration;
using InnHotel.Core.Integration.Events;
using InnHotel.Core.Interfaces;
using InnHotel.Core.ReservationAggregate;
using InnHotel.Core.RoomAggregate;
using InnHotel.Core.Services;
using Microsoft.Extensions.Logging;

namespace InnHotel.UseCases.Services;

/// <summary>
/// Implementation of integration service for handling cross-module business logic
/// </summary>
public class IntegrationService : IIntegrationService
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<IntegrationService> _logger;
    private readonly IReadRepository<Reservation> _reservationRepository;
    private readonly IReadRepository<Room> _roomRepository;
    private readonly IRepository<Reservation> _reservationWriteRepository;
    private readonly IRepository<Room> _roomWriteRepository;
    private readonly IHousekeepingService _housekeepingService;
    private readonly IBillingService _billingService;

    public IntegrationService(
        IEventBus eventBus,
        ILogger<IntegrationService> logger,
        IReadRepository<Reservation> reservationRepository,
        IReadRepository<Room> roomRepository,
        IRepository<Reservation> reservationWriteRepository,
        IRepository<Room> roomWriteRepository,
        IHousekeepingService housekeepingService,
        IBillingService billingService)
    {
        _eventBus = eventBus;
        _logger = logger;
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
        _reservationWriteRepository = reservationWriteRepository;
        _roomWriteRepository = roomWriteRepository;
        _housekeepingService = housekeepingService;
        _billingService = billingService;
    }

    public async Task ProcessGuestCheckInAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing guest check-in for reservation {ReservationId}", reservationId);

        var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken);
        if (reservation == null)
        {
            throw new InvalidOperationException($"Reservation with ID {reservationId} not found");
        }

        // Validate check-in date
        if (reservation.CheckInDate > DateOnly.FromDateTime(DateTime.Today))
        {
            throw new InvalidOperationException("Cannot check in before the reservation check-in date");
        }

        // Get room IDs from reservation
        var roomIds = reservation.Rooms.Select(r => r.RoomId).ToArray();

        // Update room statuses to Occupied
        foreach (var reservationRoom in reservation.Rooms)
        {
            var room = await _roomRepository.GetByIdAsync(reservationRoom.RoomId, cancellationToken);
            if (room != null)
            {
                var oldStatus = room.Status;
                room.UpdateStatus(RoomStatus.Occupied);
                await _roomWriteRepository.UpdateAsync(room, cancellationToken);

                // Publish room status change event
                await _eventBus.PublishAsync(new RoomStatusChangedEvent(
                    room.Id, 
                    room.RoomNumber, 
                    oldStatus, 
                    RoomStatus.Occupied, 
                    "System Check-in"), cancellationToken);
            }
        }

        // Update reservation status
        reservation.Status = ReservationStatus.CheckedIn;
        await _reservationWriteRepository.UpdateAsync(reservation, cancellationToken);

        // Publish guest check-in event
        await _eventBus.PublishAsync(new GuestCheckedInEvent(
            reservation.GuestId,
            reservation.Id,
            roomIds), cancellationToken);

        _logger.LogInformation("Guest check-in processed successfully for reservation {ReservationId}", reservationId);
    }

    public async Task ProcessGuestCheckOutAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing guest check-out for reservation {ReservationId}", reservationId);

        var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken);
        if (reservation == null)
        {
            throw new InvalidOperationException($"Reservation with ID {reservationId} not found");
        }

        // Get room IDs from reservation
        var roomIds = reservation.Rooms.Select(r => r.RoomId).ToArray();

        // Update room statuses to Cleaning
        foreach (var reservationRoom in reservation.Rooms)
        {
            var room = await _roomRepository.GetByIdAsync(reservationRoom.RoomId, cancellationToken);
            if (room != null)
            {
                var oldStatus = room.Status;
                room.UpdateStatus(RoomStatus.Cleaning);
                await _roomWriteRepository.UpdateAsync(room, cancellationToken);

                // Publish room status change event
                await _eventBus.PublishAsync(new RoomStatusChangedEvent(
                    room.Id, 
                    room.RoomNumber, 
                    oldStatus, 
                    RoomStatus.Cleaning, 
                    "System Check-out"), cancellationToken);

                // Publish housekeeping required event
                await _eventBus.PublishAsync(new HousekeepingRequiredEvent(
                    room.Id,
                    room.RoomNumber,
                    HousekeepingPriority.Normal,
                    DateTime.Now.AddHours(2)), cancellationToken);
            }
        }

        // Calculate final bill
        reservation.CalculateTotalCost();
        await _reservationWriteRepository.UpdateAsync(reservation, cancellationToken);

        // Update reservation status
        reservation.Status = ReservationStatus.CheckedOut;
        await _reservationWriteRepository.UpdateAsync(reservation, cancellationToken);

        // Publish guest check-out event
        await _eventBus.PublishAsync(new GuestCheckedOutEvent(
            reservation.GuestId,
            reservation.Id,
            reservation.TotalCost), cancellationToken);

        _logger.LogInformation("Guest check-out processed successfully for reservation {ReservationId}", reservationId);
    }

    public async Task ProcessRoomStatusChangeAsync(int roomId, RoomStatus newStatus, string? changedBy = null, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing room status change for room {RoomId} to {Status}", roomId, newStatus);

        var room = await _roomRepository.GetByIdAsync(roomId, cancellationToken);
        if (room == null)
        {
            throw new InvalidOperationException($"Room with ID {roomId} not found");
        }

        var oldStatus = room.Status;
        room.UpdateStatus(newStatus);
        await _roomWriteRepository.UpdateAsync(room, cancellationToken);

        // Publish room status change event
        await _eventBus.PublishAsync(new RoomStatusChangedEvent(
            roomId,
            room.RoomNumber,
            oldStatus,
            newStatus,
            changedBy), cancellationToken);

        // Additional logic based on new status
        switch (newStatus)
        {
            case RoomStatus.Available:
                await _eventBus.PublishAsync(new RoomBecameAvailableEvent(
                    roomId, 
                    room.RoomNumber, 
                    newStatus), cancellationToken);
                break;

            case RoomStatus.Maintenance:
                // Check for upcoming reservations
                await HandleMaintenanceModeAsync(roomId, room.RoomNumber, cancellationToken);
                break;
        }

        _logger.LogInformation("Room status change processed successfully for room {RoomId}", roomId);
    }

    public async Task ProcessReservationCreationAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing reservation creation for reservation {ReservationId}", reservationId);

        var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken);
        if (reservation == null)
        {
            throw new InvalidOperationException($"Reservation with ID {reservationId} not found");
        }

        // Validate room availability
        var validation = await ValidateBusinessRulesAsync("CreateReservation", new Dictionary<string, object>
        {
            ["ReservationId"] = reservationId,
            ["RoomIds"] = reservation.Rooms.Select(r => r.RoomId).ToArray(),
            ["CheckInDate"] = reservation.CheckInDate,
            ["CheckOutDate"] = reservation.CheckOutDate
        }, cancellationToken);

        if (!validation.IsValid)
        {
            throw new InvalidOperationException($"Reservation validation failed: {string.Join(", ", validation.Errors)}");
        }

        // Block rooms temporarily
        foreach (var reservationRoom in reservation.Rooms)
        {
            var room = await _roomRepository.GetByIdAsync(reservationRoom.RoomId, cancellationToken);
            if (room != null && room.Status == RoomStatus.Available)
            {
                var oldStatus = room.Status;
                room.UpdateStatus(RoomStatus.Reserved);
                await _roomWriteRepository.UpdateAsync(room, cancellationToken);

                await _eventBus.PublishAsync(new RoomStatusChangedEvent(
                    room.Id,
                    room.RoomNumber,
                    oldStatus,
                    RoomStatus.Reserved,
                    "Reservation Creation"), cancellationToken);
            }
        }

        // Publish reservation created event
        await _eventBus.PublishAsync(new ReservationCreatedEvent(
            reservation.Id,
            reservation.GuestId,
            reservation.CheckInDate,
            reservation.CheckOutDate,
            reservation.TotalCost,
            reservation.Rooms.Select(r => r.RoomId).ToArray(),
            reservation.Services.Select(s => s.ServiceId).ToArray()), cancellationToken);

        _logger.LogInformation("Reservation creation processed successfully for reservation {ReservationId}", reservationId);
    }

    public async Task ProcessReservationCancellationAsync(int reservationId, string reason, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing reservation cancellation for reservation {ReservationId}", reservationId);

        var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken);
        if (reservation == null)
        {
            throw new InvalidOperationException($"Reservation with ID {reservationId} not found");
        }

        var releasedRoomIds = new List<int>();

        // Release rooms
        foreach (var reservationRoom in reservation.Rooms)
        {
            var room = await _roomRepository.GetByIdAsync(reservationRoom.RoomId, cancellationToken);
            if (room != null)
            {
                var oldStatus = room.Status;
                room.UpdateStatus(RoomStatus.Available);
                await _roomWriteRepository.UpdateAsync(room, cancellationToken);
                releasedRoomIds.Add(room.Id);

                await _eventBus.PublishAsync(new RoomStatusChangedEvent(
                    room.Id,
                    room.RoomNumber,
                    oldStatus,
                    RoomStatus.Available,
                    "Reservation Cancellation"), cancellationToken);
            }
        }

        // Calculate refund amount (simplified - in real system would have cancellation policies)
        var refundAmount = reservation.TotalCost * 0.8m; // 80% refund

        // Update reservation status
        reservation.Status = ReservationStatus.Cancelled;
        await _reservationWriteRepository.UpdateAsync(reservation, cancellationToken);

        // Publish reservation cancelled event
        await _eventBus.PublishAsync(new ReservationCancelledEvent(
            reservation.Id,
            reservation.GuestId,
            reservation.CheckInDate,
            reservation.CheckOutDate,
            refundAmount,
            reason,
            releasedRoomIds.ToArray()), cancellationToken);

        _logger.LogInformation("Reservation cancellation processed successfully for reservation {ReservationId}", reservationId);
    }

    public async Task ProcessPaymentCompletionAsync(int paymentId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing payment completion for payment {PaymentId}", paymentId);

        // In a real implementation, this would:
        // 1. Update payment status
        // 2. Update reservation billing status
        // 3. Send payment confirmation
        // 4. Update financial records

        await _eventBus.PublishAsync(new PaymentProcessedEvent(
            paymentId,
            0, // Would get from payment repository
            0, // Would get from payment repository
            0, // Would get from payment repository
            0, // Would get from payment repository
            "CreditCard",
            PaymentStatus.Completed,
            Guid.NewGuid().ToString()), cancellationToken);

        _logger.LogInformation("Payment completion processed successfully for payment {PaymentId}", paymentId);
    }

    public async Task<ValidationResult> ValidateBusinessRulesAsync(string operationType, Dictionary<string, object> parameters, CancellationToken cancellationToken = default)
    {
        var result = ValidationResult.Success();

        switch (operationType)
        {
            case "CreateReservation":
                result = await ValidateReservationCreationAsync(parameters, cancellationToken);
                break;

            case "CheckIn":
                result = await ValidateCheckInAsync(parameters, cancellationToken);
                break;

            case "CheckOut":
                result = await ValidateCheckOutAsync(parameters, cancellationToken);
                break;

            default:
                result = ValidationResult.Failure($"Unknown operation type: {operationType}");
                break;
        }

        return result;
    }

    private async Task<ValidationResult> ValidateReservationCreationAsync(Dictionary<string, object> parameters, CancellationToken cancellationToken)
    {
        var result = ValidationResult.Success();

        if (parameters.TryGetValue("RoomIds", out var roomIdsObj) && roomIdsObj is int[] roomIds)
        {
            foreach (var roomId in roomIds)
            {
                var room = await _roomRepository.GetByIdAsync(roomId, cancellationToken);
                if (room == null)
                {
                    result.Errors.Add($"Room with ID {roomId} not found");
                }
                else if (room.Status != RoomStatus.Available)
                {
                    result.Errors.Add($"Room {room.RoomNumber} is not available (current status: {room.Status})");
                }
            }
        }

        if (parameters.TryGetValue("CheckInDate", out var checkInObj) && checkInObj is DateOnly checkInDate &&
            parameters.TryGetValue("CheckOutDate", out var checkOutObj) && checkOutObj is DateOnly checkOutDate)
        {
            if (checkInDate >= checkOutDate)
            {
                result.Errors.Add("Check-in date must be before check-out date");
            }

            if (checkInDate < DateOnly.FromDateTime(DateTime.Today))
            {
                result.Warnings.Add("Check-in date is in the past");
            }
        }

        result.IsValid = !result.Errors.Any();
        return result;
    }

    private async Task<ValidationResult> ValidateCheckInAsync(Dictionary<string, object> parameters, CancellationToken cancellationToken)
    {
        var result = ValidationResult.Success();

        if (parameters.TryGetValue("ReservationId", out var reservationIdObj) && reservationIdObj is int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken);
            if (reservation == null)
            {
                result.Errors.Add($"Reservation with ID {reservationId} not found");
            }
            else if (reservation.Status != ReservationStatus.Confirmed)
            {
                result.Errors.Add($"Cannot check in reservation with status {reservation.Status}");
            }
            else if (reservation.CheckInDate > DateOnly.FromDateTime(DateTime.Today))
            {
                result.Errors.Add("Cannot check in before the reservation check-in date");
            }
        }

        result.IsValid = !result.Errors.Any();
        return result;
    }

    private async Task<ValidationResult> ValidateCheckOutAsync(Dictionary<string, object> parameters, CancellationToken cancellationToken)
    {
        var result = ValidationResult.Success();

        if (parameters.TryGetValue("ReservationId", out var reservationIdObj) && reservationIdObj is int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken);
            if (reservation == null)
            {
                result.Errors.Add($"Reservation with ID {reservationId} not found");
            }
            else if (reservation.Status != ReservationStatus.CheckedIn)
            {
                result.Errors.Add($"Cannot check out reservation with status {reservation.Status}");
            }
        }

        result.IsValid = !result.Errors.Any();
        return result;
    }

    private async Task HandleMaintenanceModeAsync(int roomId, string roomNumber, CancellationToken cancellationToken)
    {
        // Check for upcoming reservations for this room
        // In a real implementation, this would query the reservation repository
        // for reservations that include this room in the future

        _logger.LogWarning("Room {RoomNumber} placed in maintenance mode - checking for conflicts", roomNumber);
    }
}