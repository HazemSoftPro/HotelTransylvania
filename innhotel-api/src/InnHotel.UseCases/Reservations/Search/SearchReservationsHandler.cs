using InnHotel.Core.ReservationAggregate;
using InnHotel.Core.ReservationAggregate.Specifications;

namespace InnHotel.UseCases.Reservations.Search;

public class SearchReservationsHandler(IReadRepository<Reservation> _repository)
    : IQueryHandler<SearchReservationsQuery, Result<IEnumerable<ReservationDto>>>
{
    public async Task<Result<IEnumerable<ReservationDto>>> Handle(SearchReservationsQuery request, CancellationToken cancellationToken)
    {
        var spec = new ReservationSearchSpec(
            request.SearchTerm,
            request.GuestId,
            request.Status,
            request.CheckInDateFrom,
            request.CheckInDateTo,
            request.CheckOutDateFrom,
            request.CheckOutDateTo,
            request.PageNumber,
            request.PageSize);

        var reservations = await _repository.ListAsync(spec, cancellationToken);

        var reservationDtos = reservations.Select(r =>
        {
            var firstRoom = r.Rooms.FirstOrDefault();
            return new ReservationDto
            {
                Id = r.Id,
                GuestId = r.GuestId,
                GuestName = $"{r.Guest.FirstName} {r.Guest.LastName}",
                BranchId = firstRoom?.Room?.BranchId,
                BranchName = firstRoom?.Room?.Branch?.Name ?? "Unknown",
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                ReservationDate = r.ReservationDate,
                Status = r.Status,
                TotalCost = r.TotalCost,
                Rooms = r.Rooms.Select(rr => new ReservationRoomDto
                {
                    RoomId = rr.RoomId,
                    RoomNumber = rr.Room?.RoomNumber ?? "Unknown",
                    RoomTypeName = rr.Room?.RoomType?.Name ?? "Unknown",
                    PricePerNight = rr.PricePerNight
                }).ToList(),
                Services = r.Services.Select(rs => new ReservationServiceDto
                {
                    ServiceId = rs.ServiceId,
                    ServiceName = rs.Service?.Name ?? "Service",
                    Quantity = rs.Quantity,
                    UnitPrice = rs.UnitPrice
                }).ToList()
            };
        }).ToList();

        return Result.Success(reservationDtos.AsEnumerable());
    }
}
