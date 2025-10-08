using InnHotel.Core.ReservationAggregate;
using InnHotel.Core.ReservationAggregate.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InnHotel.UseCases.Reservations.List;

public class ListReservationsHandler : IRequestHandler<ListReservationsQuery, Result<(List<ReservationDto> Items, int TotalCount)>>
{
    private readonly IReadRepository<Reservation> _repository;
    private readonly ILogger<ListReservationsHandler> _logger;

    public ListReservationsHandler(
        IReadRepository<Reservation> repository,
        ILogger<ListReservationsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<(List<ReservationDto> Items, int TotalCount)>> Handle(
        ListReservationsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var totalCount = await _repository.CountAsync(cancellationToken);
            
            var spec = new ReservationsPaginatedSpec(request.PageNumber, request.PageSize);
            var reservations = await _repository.ListAsync(spec, cancellationToken);

            var reservationDtos = reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                GuestId = r.GuestId,
                GuestName = r.Guest != null ? $"{r.Guest.FirstName} {r.Guest.LastName}" : "Unknown",
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                ReservationDate = r.ReservationDate,
                Status = r.Status,
                TotalCost = r.TotalCost,
                Rooms = r.Rooms.Select(room => new ReservationRoomDto
                {
                    RoomId = room.RoomId,
                    RoomNumber = room.Room?.RoomNumber ?? "Unknown",
                    RoomTypeName = room.Room?.RoomType?.Name ?? "Unknown",
                    PricePerNight = room.PricePerNight
                }).ToList(),
                Services = r.Services.Select(service => new ReservationServiceDto
                {
                    ServiceId = service.ServiceId,
                    ServiceName = service.Service?.Name ?? "Service",
                    Quantity = service.Quantity,
                    UnitPrice = service.UnitPrice
                }).ToList()
            }).ToList();

            return (reservationDtos, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing reservations");
            return Result.Error("An error occurred while listing reservations.");
        }
    }
}
