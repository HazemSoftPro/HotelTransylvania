using Ardalis.Specification;

namespace InnHotel.Core.EmployeeAggregate.Specifications;

public sealed class EmployeeSearchSpec : Specification<Employee>
{
    public EmployeeSearchSpec(
        string? searchTerm = null,
        int? branchId = null,
        string? position = null,
        DateTime? hireDateFrom = null,
        DateTime? hireDateTo = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        Query
            .Include(e => e.Branch)
            .Include(e => e.User);

        // Apply search term filter (searches first name, last name, position, and email)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerSearchTerm = searchTerm.ToLower();
            Query.Where(e => e.FirstName.ToLower().Contains(lowerSearchTerm) ||
                           e.LastName.ToLower().Contains(lowerSearchTerm) ||
                           e.Position.ToLower().Contains(lowerSearchTerm) ||
                           (e.User != null && e.User.Email != null && e.User.Email.ToLower().Contains(lowerSearchTerm)));
        }

        // Apply branch filter
        if (branchId.HasValue)
        {
            Query.Where(e => e.BranchId == branchId.Value);
        }

        // Apply position filter
        if (!string.IsNullOrWhiteSpace(position))
        {
            var lowerPosition = position.ToLower();
            Query.Where(e => e.Position.ToLower().Contains(lowerPosition));
        }

        // Apply hire date range filter
        if (hireDateFrom.HasValue)
        {
            Query.Where(e => e.HireDate >= DateOnly.FromDateTime(hireDateFrom.Value));
        }

        if (hireDateTo.HasValue)
        {
            Query.Where(e => e.HireDate <= DateOnly.FromDateTime(hireDateTo.Value));
        }

        // Apply pagination and ordering
        Query
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
