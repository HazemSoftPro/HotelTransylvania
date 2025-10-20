using Ardalis.Specification;

namespace InnHotel.Core.PaymentAggregate.Specifications;

public class PaymentsByReservationIdSpec : Specification<Payment>
{
    public PaymentsByReservationIdSpec(int reservationId)
    {
        Query
            .Where(p => p.ReservationId == reservationId)
            .OrderByDescending(p => p.PaymentDate);
    }
}