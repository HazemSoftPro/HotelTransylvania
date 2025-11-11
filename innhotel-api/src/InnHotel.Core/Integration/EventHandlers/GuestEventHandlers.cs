using InnHotel.Core.Integration.Events;
using InnHotel.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace InnHotel.Core.Integration.EventHandlers;

/// <summary>
/// Handles guest-related integration events
/// </summary>
public class GuestCreatedEventHandler : IIntegrationEventHandler<GuestCreatedEvent>
{
    private readonly ILogger<GuestCreatedEventHandler> _logger;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;

    public GuestCreatedEventHandler(
        ILogger<GuestCreatedEventHandler> logger,
        IEmailService emailService,
        INotificationService notificationService)
    {
        _logger = logger;
        _emailService = emailService;
        _notificationService = notificationService;
    }

    public async Task HandleAsync(GuestCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling GuestCreatedEvent for GuestId: {GuestId}", @event.GuestId);

        // Send welcome email if email is provided
        if (!string.IsNullOrEmpty(@event.Email))
        {
            await _emailService.SendWelcomeEmailAsync(@event.Email, @event.FirstName, cancellationToken);
        }

        // Create notification for front desk staff
        await _notificationService.CreateNotificationAsync(
            "New Guest Registered",
            $"Guest {@event.FirstName} {@event.LastName} has been registered in the system.",
            "GuestManagement",
            new Dictionary<string, object>
            {
                ["GuestId"] = @event.GuestId,
                ["GuestName"] = $"{@event.FirstName} {@event.LastName}"
            },
            cancellationToken);

        _logger.LogInformation("GuestCreatedEvent processed successfully for GuestId: {GuestId}", @event.GuestId);
    }
}

/// <summary>
/// Handles guest check-in events
/// </summary>
public class GuestCheckedInEventHandler : IIntegrationEventHandler<GuestCheckedInEvent>
{
    private readonly ILogger<GuestCheckedInEventHandler> _logger;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;
    private readonly IHousekeepingService _housekeepingService;

    public GuestCheckedInEventHandler(
        ILogger<GuestCheckedInEventHandler> logger,
        IEmailService emailService,
        INotificationService notificationService,
        IHousekeepingService housekeepingService)
    {
        _logger = logger;
        _emailService = emailService;
        _notificationService = notificationService;
        _housekeepingService = housekeepingService;
    }

    public async Task HandleAsync(GuestCheckedInEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling GuestCheckedInEvent for GuestId: {GuestId}, ReservationId: {ReservationId}", 
            @event.GuestId, @event.ReservationId);

        // Update room statuses to Occupied
        foreach (var roomId in @event.RoomIds)
        {
            await _housekeepingService.MarkRoomAsOccupiedAsync(roomId, cancellationToken);
        }

        // Create notification for housekeeping staff
        await _notificationService.CreateNotificationAsync(
            "Guest Check-in Completed",
            $"Guest has checked in to rooms: {string.Join(", ", @event.RoomIds)}",
            "Housekeeping",
            new Dictionary<string, object>
            {
                ["GuestId"] = @event.GuestId,
                ["ReservationId"] = @event.ReservationId,
                ["RoomIds"] = @event.RoomIds,
                ["CheckInTime"] = @event.CheckInTime
            },
            cancellationToken);

        // Create notification for management
        await _notificationService.CreateNotificationAsync(
            "New Guest Arrival",
            $"Guest has checked in for Reservation ID: {@event.ReservationId}",
            "Management",
            new Dictionary<string, object>
            {
                ["GuestId"] = @event.GuestId,
                ["ReservationId"] = @event.ReservationId
            },
            cancellationToken);

        _logger.LogInformation("GuestCheckedInEvent processed successfully");
    }
}

/// <summary>
/// Handles guest check-out events
/// </summary>
public class GuestCheckedOutEventHandler : IIntegrationEventHandler<GuestCheckedOutEvent>
{
    private readonly ILogger<GuestCheckedOutEventHandler> _logger;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;
    private readonly IHousekeepingService _housekeepingService;
    private readonly IBillingService _billingService;

    public GuestCheckedOutEventHandler(
        ILogger<GuestCheckedOutEventHandler> logger,
        IEmailService emailService,
        INotificationService notificationService,
        IHousekeepingService housekeepingService,
        IBillingService billingService)
    {
        _logger = logger;
        _emailService = emailService;
        _notificationService = notificationService;
        _housekeepingService = housekeepingService;
        _billingService = billingService;
    }

    public async Task HandleAsync(GuestCheckedOutEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling GuestCheckedOutEvent for GuestId: {GuestId}, ReservationId: {ReservationId}", 
            @event.GuestId, @event.ReservationId);

        // Finalize billing
        await _billingService.FinalizeBillAsync(@event.ReservationId, @event.TotalBill, cancellationToken);

        // Mark rooms for housekeeping
        await _housekeepingService.ScheduleCleaningAsync(@event.ReservationId, cancellationToken);

        // Send thank you email (would need guest email from reservation)
        // await _emailService.SendThankYouEmailAsync(guestEmail, cancellationToken);

        // Create notification for housekeeping staff
        await _notificationService.CreateNotificationAsync(
            "Rooms Ready for Cleaning",
            $"Rooms from Reservation ID: {@event.ReservationId} need cleaning after guest check-out",
            "Housekeeping",
            new Dictionary<string, object>
            {
                ["GuestId"] = @event.GuestId,
                ["ReservationId"] = @event.ReservationId,
                ["TotalBill"] = @event.TotalBill,
                ["CheckOutTime"] = @event.CheckOutTime
            },
            cancellationToken);

        // Create notification for accounting
        await _notificationService.CreateNotificationAsync(
            "Guest Check-out Completed",
            $"Final bill of {@event.TotalBill:C} processed for Reservation ID: {@event.ReservationId}",
            "Accounting",
            new Dictionary<string, object>
            {
                ["GuestId"] = @event.GuestId,
                ["ReservationId"] = @event.ReservationId,
                ["TotalBill"] = @event.TotalBill
            },
            cancellationToken);

        _logger.LogInformation("GuestCheckedOutEvent processed successfully");
    }
}