using Ardalis.Specification;

namespace InnHotel.Core.ReservationAggregate.Specifications;

public sealed class ReservationSearchSpec : Specification<Reservation>
{
    public ReservationSearchSpec(
        string? searchTerm = null,
        int? guestId = null,
        ReservationStatus? status = null,
        DateTime? checkInDateFrom = null,
        DateTime? checkInDateTo = null,
        DateTime? checkOutDateFrom = null,
        DateTime? checkOutDateTo = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        Query
            .Include(r => r.Guest)
            .Include(r => r.ReservationRooms)
                .ThenInclude(rr => rr.Room)
                .ThenInclude(r => r.RoomType)
            .Include(r => r.ReservationRooms)
                .ThenInclude(rr => rr.Room)
                .ThenInclude(r => r.Branch);

        // Apply search term filter (searches guest name and room numbers)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerSearchTerm = searchTerm.ToLower();
            Query.Where(r => r.Guest.FirstName.ToLower().Contains(lowerSearchTerm) ||
                           r.Guest.LastName.ToLower().Contains(lowerSearchTerm) ||
                           r.ReservationRooms.Any(rr => rr.Room.RoomNumber.Contains(searchTerm)));
        }

        // Apply guest filter
        if (guestId.HasValue)
        {
            Query.Where(r => r.GuestId == guestId.Value);
        }

        // Apply status filter
        if (status.HasValue)
        {
            Query.Where(r => r.Status == status.Value);
        }

        // Apply check-in date range filter
        if (checkInDateFrom.HasValue)
        {
            Query.Where(r => r.CheckInDate >= checkInDateFrom.Value);
        }

        if (checkInDateTo.HasValue)
        {
            Query.Where(r => r.CheckInDate <= checkInDateTo.Value);
        }

        // Apply check-out date range filter
        if (checkOutDateFrom.HasValue)
        {
            Query.Where(r => r.CheckOutDate >= checkOutDateFrom.Value);
        }

        if (checkOutDateTo.HasValue)
        {
            Query.Where(r => r.CheckOutDate <= checkOutDateTo.Value);
        }

        // Apply pagination and ordering
        Query
            .OrderByDescending(r => r.CheckInDate)
            .ThenBy(r => r.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
