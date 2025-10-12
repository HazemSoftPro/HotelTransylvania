using InnHotel.Core.GuestAggregate;
using InnHotel.Core.GuestAggregate.Specifications;
using InnHotel.Core.ReservationAggregate;

namespace InnHotel.UseCases.Guests.GetHistory;

public class GetGuestHistoryHandler(
    IReadRepository<Guest> _guestRepository,
    IReadRepository<Reservation> _reservationRepository)
    : IQueryHandler<GetGuestHistoryQuery, Result<IEnumerable<ReservationDTO>>>
{
    public async Task<Result<IEnumerable<ReservationDTO>>> Handle(GetGuestHistoryQuery request, CancellationToken cancellationToken)
    {
        // First verify the guest exists
        var guest = await _guestRepository.GetByIdAsync(request.GuestId, cancellationToken);
        if (guest == null)
        {
            return Result.NotFound($"Guest with ID {request.GuestId} not found.");
        }

        // Get guest's reservation history
        var spec = new GuestHistorySpec(request.GuestId, request.PageNumber, request.PageSize);
        var reservations = await _reservationRepository.ListAsync(spec, cancellationToken);

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
