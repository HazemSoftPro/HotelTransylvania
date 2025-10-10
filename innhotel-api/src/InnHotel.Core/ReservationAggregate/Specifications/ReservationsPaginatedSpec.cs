using Ardalis.Specification;

namespace InnHotel.Core.ReservationAggregate.Specifications;

public class ReservationsPaginatedSpec : Specification<Reservation>
{
    public ReservationsPaginatedSpec(int pageNumber, int pageSize)
    {
        Query
            .Include(r => r.Guest)
            .Include(r => r.Rooms)
                .ThenInclude(rr => rr.Room)
                    .ThenInclude(r => r.RoomType)
            .Include(r => r.Rooms)
                .ThenInclude(rr => rr.Room)
                    .ThenInclude(r => r.Branch)
            .Include(r => r.Services)
                .ThenInclude(rs => rs.Service)
            .OrderByDescending(r => r.ReservationDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
