using InnHotel.Core.ReservationAggregate;
using InnHotel.Core.ReservationAggregate.Specifications;

namespace InnHotel.UseCases.Reservations.Search;

public class SearchReservationsHandler(IReadRepository<Reservation> _repository)
    : IQueryHandler<SearchReservationsQuery, Result<IEnumerable<ReservationDTO>>>
{
    public async Task<Result<IEnumerable<ReservationDTO>>> Handle(SearchReservationsQuery request, CancellationToken cancellationToken)
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

        var reservationDtos = reservations.Select(r => new ReservationDTO(
            r.Id,
            r.GuestId,
            $"{r.Guest.FirstName} {r.Guest.LastName}",
            r.CheckInDate,
            r.CheckOutDate,
            r.Status,
            r.ReservationRooms.Select(rr => new ReservationRoomDTO(
                rr.RoomId,
                rr.Room.RoomNumber,
                rr.Room.RoomType.Name,
                rr.Room.Branch.Name)).ToList(),
            r.ReservationServices.Select(rs => new ReservationServiceDTO(
                rs.ServiceId,
                rs.Service.Name,
                rs.Service.Price)).ToList())).ToList();

        return Result.Success(reservationDtos.AsEnumerable());
    }
}
