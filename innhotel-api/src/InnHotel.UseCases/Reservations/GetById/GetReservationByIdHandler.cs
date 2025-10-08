using InnHotel.Core.ReservationAggregate;
using InnHotel.Core.ReservationAggregate.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InnHotel.UseCases.Reservations.GetById;

public class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery, Result<ReservationDto>>
{
    private readonly IReadRepository<Reservation> _repository;
    private readonly ILogger<GetReservationByIdHandler> _logger;

    public GetReservationByIdHandler(
        IReadRepository<Reservation> repository,
        ILogger<GetReservationByIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<ReservationDto>> Handle(
        GetReservationByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var spec = new ReservationByIdSpec(request.Id);
            var reservation = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

            if (reservation == null)
            {
                return Result<ReservationDto>.NotFound($"Reservation with ID {request.Id} not found.");
            }

            var dto = new ReservationDto
            {
                Id = reservation.Id,
                GuestId = reservation.GuestId,
                GuestName = reservation.Guest != null ? $"{reservation.Guest.FirstName} {reservation.Guest.LastName}" : "Unknown",
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                ReservationDate = reservation.ReservationDate,
                Status = reservation.Status,
                TotalCost = reservation.TotalCost,
                Rooms = reservation.Rooms.Select(room => new ReservationRoomDto
                {
                    RoomId = room.RoomId,
                    RoomNumber = room.Room?.RoomNumber ?? "Unknown",
                    RoomTypeName = room.Room?.RoomType?.Name ?? "Unknown",
                    PricePerNight = room.PricePerNight
                }).ToList(),
                Services = reservation.Services.Select(service => new ReservationServiceDto
                {
                    ServiceId = service.ServiceId,
                    ServiceName = service.Service?.Name ?? "Service",
                    Quantity = service.Quantity,
                    UnitPrice = service.UnitPrice
                }).ToList()
            };

            return Result<ReservationDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reservation by ID {ReservationId}", request.Id);
            return Result<ReservationDto>.Error("An error occurred while fetching the reservation.");
        }
    }
}
