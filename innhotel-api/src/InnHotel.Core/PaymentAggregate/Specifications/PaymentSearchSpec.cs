using Ardalis.Specification;

namespace InnHotel.Core.PaymentAggregate.Specifications;

public class PaymentSearchSpec : Specification<Payment>
{
    public PaymentSearchSpec(
        PaymentStatus? status = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int? reservationId = null,
        int skip = 0,
        int take = 20)
    {
        Query
            .Include(p => p.Reservation)
                .ThenInclude(r => r.Guest);

        if (status.HasValue)
        {
            Query.Where(p => p.Status == status.Value);
        }

        if (startDate.HasValue)
        {
            Query.Where(p => p.PaymentDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            Query.Where(p => p.PaymentDate <= endDate.Value);
        }

        if (reservationId.HasValue)
        {
            Query.Where(p => p.ReservationId == reservationId.Value);
        }

        Query
            .OrderByDescending(p => p.PaymentDate)
            .Skip(skip)
            .Take(take);
    }
}