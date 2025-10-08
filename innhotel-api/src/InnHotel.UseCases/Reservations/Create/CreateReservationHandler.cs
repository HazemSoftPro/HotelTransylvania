using InnHotel.Core.ReservationAggregate;
using InnHotel.Core.ReservationAggregate.Specifications;
using InnHotel.Core.GuestAggregate;
using InnHotel.Core.GuestAggregate.Specifications;
using InnHotel.Core.RoomAggregate;
using InnHotel.Core.RoomAggregate.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InnHotel.UseCases.Reservations.Create;

public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, Result<ReservationDto>>
{
    private readonly IRepository<Reservation> _repository;
    private readonly IRepository<Guest> _guestRepository;
    private readonly IRepository<Room> _roomRepository;
    private readonly ILogger<CreateReservationHandler> _logger;

    public CreateReservationHandler(
        IRepository<Reservation> repository,
        IRepository<Guest> guestRepository,
        IRepository<Room> roomRepository,
        ILogger<CreateReservationHandler> logger)
    {
        _repository = repository;
        _guestRepository = guestRepository;
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task<Result<ReservationDto>> Handle(
        CreateReservationCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Validate guest exists
            var guestSpec = new GuestByIdSpec(request.GuestId);
            var guest = await _guestRepository.FirstOrDefaultAsync(guestSpec, cancellationToken);
            if (guest == null)
            {
                return Result<ReservationDto>.NotFound($"Guest with ID {request.GuestId} not found.");
            }

            // Validate rooms exist and are available
            var roomIds = request.Rooms.Select(r => r.RoomId).ToList();
            var roomsSpec = new RoomsByIdsSpec(roomIds);
            var rooms = await _roomRepository.ListAsync(roomsSpec, cancellationToken);
            
            if (rooms.Count != roomIds.Count)
            {
                return Result<ReservationDto>.NotFound("One or more rooms not found.");
            }

            // Check room availability
            foreach (var room in rooms)
            {
                if (room.Status != RoomStatus.Available)
                {
                    return Result<ReservationDto>.Error($"Room {room.RoomNumber} is not available.");
                }
            }

            // Create reservation
            var reservation = new Reservation
            {
                GuestId = request.GuestId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                ReservationDate = DateTime.UtcNow,
                Status = ReservationStatus.Confirmed
            };

            // Add rooms to reservation
            foreach (var roomRequest in request.Rooms)
            {
                var reservationRoom = new ReservationRoom
                {
                    RoomId = roomRequest.RoomId,
                    PricePerNight = roomRequest.PricePerNight
                };
                reservation.AddRoom(reservationRoom);
            }

            // Add services if any
            if (request.Services != null && request.Services.Any())
            {
                foreach (var serviceRequest in request.Services)
                {
                    var reservationService = new ReservationService
                    {
                        ServiceId = serviceRequest.ServiceId,
                        Quantity = serviceRequest.Quantity,
                        UnitPrice = serviceRequest.UnitPrice,
                        TotalPrice = serviceRequest.Quantity * serviceRequest.UnitPrice
                    };
                    reservation.AddService(reservationService);
                }
            }

            // Calculate total cost
            reservation.CalculateTotalCost();

            // Save reservation
            await _repository.AddAsync(reservation, cancellationToken);

            _logger.LogInformation("Created reservation {ReservationId} for guest {GuestId}", 
                reservation.Id, reservation.GuestId);

            // Map to DTO
            var dto = new ReservationDto
            {
                Id = reservation.Id,
                GuestId = reservation.GuestId,
                GuestName = $"{guest.FirstName} {guest.LastName}",
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                ReservationDate = reservation.ReservationDate,
                Status = reservation.Status,
                TotalCost = reservation.TotalCost,
                Rooms = reservation.Rooms.Select(r => 
                {
                    var room = rooms.First(rm => rm.Id == r.RoomId);
                    return new ReservationRoomDto
                    {
                        RoomId = r.RoomId,
                        RoomNumber = room.RoomNumber,
                        RoomTypeName = room.RoomType?.Name ?? "Unknown",
                        PricePerNight = r.PricePerNight
                    };
                }).ToList(),
                Services = reservation.Services.Select(s => new ReservationServiceDto
                {
                    ServiceId = s.ServiceId,
                    ServiceName = "Service", // You may need to fetch service details
                    Quantity = s.Quantity,
                    UnitPrice = s.UnitPrice
                }).ToList()
            };

            return Result<ReservationDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reservation");
            return Result<ReservationDto>.Error("An error occurred while creating the reservation.");
        }
    }
}
