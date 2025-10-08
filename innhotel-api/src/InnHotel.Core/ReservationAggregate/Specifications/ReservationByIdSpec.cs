using Ardalis.Specification;

namespace InnHotel.Core.ReservationAggregate.Specifications;

public class ReservationByIdSpec : Specification<Reservation>
{
    public ReservationByIdSpec(int reservationId)
    {
        Query
            .Where(r => r.Id == reservationId)
            .Include(r => r.Guest)
            .Include(r => r.Rooms)
                .ThenInclude(rr => rr.Room)
                    .ThenInclude(r => r.RoomType)
            .Include(r => r.Services)
                .ThenInclude(rs => rs.Service);
    }
}
