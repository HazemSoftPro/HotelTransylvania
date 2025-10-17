using Ardalis.Specification;

namespace InnHotel.Core.ServiceAggregate.Specifications;

public sealed class ServiceSearchSpec : Specification<Service>
{
    public ServiceSearchSpec(
        string? searchTerm = null,
        int? branchId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        Query.Include(s => s.Branch);

        // Apply search term filter (searches name and description)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            Query.Where(s => s.Name.Contains(searchTerm) || 
                            (s.Description != null && s.Description.Contains(searchTerm)));
        }

        // Apply branch filter
        if (branchId.HasValue)
        {
            Query.Where(s => s.BranchId == branchId.Value);
        }

        // Apply minimum price filter
        if (minPrice.HasValue)
        {
            Query.Where(s => s.Price >= minPrice.Value);
        }

        // Apply maximum price filter
        if (maxPrice.HasValue)
        {
            Query.Where(s => s.Price <= maxPrice.Value);
        }

        // Apply pagination
        Query
            .OrderBy(s => s.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}