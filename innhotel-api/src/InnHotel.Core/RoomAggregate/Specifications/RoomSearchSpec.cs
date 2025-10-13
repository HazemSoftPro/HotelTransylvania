using Ardalis.Specification;

namespace InnHotel.Core.RoomAggregate.Specifications;

public sealed class RoomSearchSpec : Specification<Room>
{
    public RoomSearchSpec(
        string? searchTerm = null,
        int? branchId = null,
        int? roomTypeId = null,
        RoomStatus? status = null,
        int? floor = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        Query
            .Include(r => r.Branch)
            .Include(r => r.RoomType);

        // Apply search term filter (searches room number and room type name)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            Query.Where(r => r.RoomNumber.Contains(searchTerm) || 
                           r.RoomType.Name.Contains(searchTerm));
        }

        // Apply branch filter
        if (branchId.HasValue)
        {
            Query.Where(r => r.BranchId == branchId.Value);
        }

        // Apply room type filter
        if (roomTypeId.HasValue)
        {
            Query.Where(r => r.RoomTypeId == roomTypeId.Value);
        }

        // Apply status filter
        if (status.HasValue)
        {
            Query.Where(r => r.Status == status.Value);
        }

        // Apply floor filter
        if (floor.HasValue)
        {
            Query.Where(r => r.Floor == floor.Value);
        }

        // Apply pagination
        Query
            .OrderBy(r => r.RoomNumber)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
