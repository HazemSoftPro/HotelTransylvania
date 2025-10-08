using InnHotel.Core.RoomAggregate;

namespace InnHotel.Core.ReservationAggregate;

public class ReservationRoom : EntityBase
{
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public decimal PricePerNight { get; set; }

    public ReservationRoom()
    {
    }

    public ReservationRoom(int reservationId, int roomId, decimal pricePerNight)
    {
        ReservationId = Guard.Against.NegativeOrZero(reservationId, nameof(reservationId));
        RoomId = Guard.Against.NegativeOrZero(roomId, nameof(roomId));
        PricePerNight = Guard.Against.NegativeOrZero(pricePerNight, nameof(pricePerNight));
    }
}
