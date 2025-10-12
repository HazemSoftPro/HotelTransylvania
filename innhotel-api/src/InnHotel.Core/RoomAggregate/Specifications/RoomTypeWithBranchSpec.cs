using Ardalis.Specification;

namespace InnHotel.Core.RoomAggregate.Specifications;

public sealed class RoomTypeWithBranchSpec : Specification<RoomType>
{
    public RoomTypeWithBranchSpec()
    {
        Query.Include(rt => rt.Branch);
    }
}

public sealed class RoomTypeByIdWithBranchSpec : Specification<RoomType>
{
    public RoomTypeByIdWithBranchSpec(int id)
    {
        Query
            .Where(rt => rt.Id == id)
            .Include(rt => rt.Branch);
    }
}
