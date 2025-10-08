using InnHotel.Core.ServiceAggregate;

namespace InnHotel.Core.ReservationAggregate;

public class ReservationService : EntityBase
{
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;
    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public ReservationService()
    {
    }

    public ReservationService(int reservationId, int serviceId, int quantity, decimal totalPrice)
    {
        ReservationId = reservationId;
        ServiceId = Guard.Against.NegativeOrZero(serviceId, nameof(serviceId));
        Quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
        TotalPrice = Guard.Against.NegativeOrZero(totalPrice, nameof(totalPrice));
        UnitPrice = quantity > 0 ? totalPrice / quantity : 0;
    }
}
