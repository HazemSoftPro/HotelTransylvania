using Ardalis.Specification;

namespace InnHotel.Core.PaymentAggregate.Specifications;

public class PaymentByIdSpec : Specification<Payment>
{
    public PaymentByIdSpec(int paymentId)
    {
        Query
            .Where(p => p.Id == paymentId)
            .Include(p => p.Reservation)
                .ThenInclude(r => r.Guest);
    }
}