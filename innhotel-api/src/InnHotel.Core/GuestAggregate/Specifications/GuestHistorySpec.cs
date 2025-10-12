using Ardalis.Specification;
using InnHotel.Core.ReservationAggregate;

namespace InnHotel.Core.GuestAggregate.Specifications;

public sealed class GuestHistorySpec : Specification<Reservation>
{
    public GuestHistorySpec(int guestId, int pageNumber = 1, int pageSize = 10)
    {
        Query
            .Where(r => r.GuestId == guestId)
            .Include(r => r.Guest)
            .Include(r => r.ReservationRooms)
                .ThenInclude(rr => rr.Room)
                .ThenInclude(r => r.RoomType)
            .Include(r => r.ReservationRooms)
                .ThenInclude(rr => rr.Room)
                .ThenInclude(r => r.Branch)
            .Include(r => r.ReservationServices)
                .ThenInclude(rs => rs.Service)
            .OrderByDescending(r => r.CheckInDate)
            .ThenBy(r => r.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
