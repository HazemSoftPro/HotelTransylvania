using Ardalis.Specification;
using InnHotel.Core.GuestAggregate.ValueObjects;

namespace InnHotel.Core.GuestAggregate.Specifications;

public sealed class GuestSearchSpec : Specification<Guest>
{
    public GuestSearchSpec(
        string? searchTerm = null,
        Gender? gender = null,
        IdProofType? idProofType = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        // Apply search term filter (searches first name, last name, email, phone, and ID proof number)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerSearchTerm = searchTerm.ToLower();
            Query.Where(g => g.FirstName.ToLower().Contains(lowerSearchTerm) ||
                           g.LastName.ToLower().Contains(lowerSearchTerm) ||
                           (g.Email != null && g.Email.ToLower().Contains(lowerSearchTerm)) ||
                           (g.Phone != null && g.Phone.Contains(searchTerm)) ||
                           (g.IdProofNumber != null && g.IdProofNumber.Contains(searchTerm)));
        }

        // Apply gender filter
        if (gender.HasValue)
        {
            Query.Where(g => g.Gender == gender.Value);
        }

        // Apply ID proof type filter
        if (idProofType.HasValue)
        {
            Query.Where(g => g.IdProofType == idProofType.Value);
        }

        // Apply pagination and ordering
        Query
            .OrderBy(g => g.LastName)
            .ThenBy(g => g.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
