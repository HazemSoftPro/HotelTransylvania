using Ardalis.Specification;

namespace InnHotel.Core.RoomAggregate.Specifications;

public class RoomsByIdsSpec : Specification<Room>
{
    public RoomsByIdsSpec(List<int> roomIds)
    {
        Query
            .Where(room => roomIds.Contains(room.Id))
            .Include(room => room.RoomType);
    }
}
