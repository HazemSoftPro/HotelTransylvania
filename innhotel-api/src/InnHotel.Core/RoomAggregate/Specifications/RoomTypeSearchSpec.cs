using Ardalis.Specification;

namespace InnHotel.Core.RoomAggregate.Specifications;

public sealed class RoomTypeSearchSpec : Specification<RoomType>
{
    public RoomTypeSearchSpec(
        string? searchTerm = null,
        int? branchId = null,
        int? minCapacity = null,
        int? maxCapacity = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        Query.Include(rt => rt.Branch);

        // Apply search term filter (searches name and description)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            Query.Where(rt => rt.Name.Contains(searchTerm) || 
                             (rt.Description != null && rt.Description.Contains(searchTerm)));
        }

        // Apply branch filter
        if (branchId.HasValue)
        {
            Query.Where(rt => rt.BranchId == branchId.Value);
        }

        // Apply minimum capacity filter
        if (minCapacity.HasValue)
        {
            Query.Where(rt => rt.Capacity >= minCapacity.Value);
        }

        // Apply maximum capacity filter
        if (maxCapacity.HasValue)
        {
            Query.Where(rt => rt.Capacity <= maxCapacity.Value);
        }

        // Apply pagination
        Query
            .OrderBy(rt => rt.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}