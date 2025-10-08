using InnHotel.Core.GuestAggregate;

namespace InnHotel.Core.ReservationAggregate;

public class Reservation : EntityBase, IAggregateRoot
{
    public int GuestId { get; set; }
    public Guest Guest { get; set; } = null!;
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
    public ReservationStatus Status { get; set; }
    public decimal TotalCost { get; set; }

    private readonly List<ReservationRoom> _rooms = new();
    private readonly List<ReservationService> _services = new();
    public IReadOnlyCollection<ReservationRoom> Rooms => _rooms.AsReadOnly();
    public IReadOnlyCollection<ReservationService> Services => _services.AsReadOnly();

    public Reservation()
    {
    }

    public Reservation(
        int guestId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        ReservationStatus status,
        decimal totalCost)
    {
        GuestId = Guard.Against.NegativeOrZero(guestId, nameof(guestId));
        CheckInDate = checkInDate;
        CheckOutDate = Guard.Against.OutOfRange(
            checkOutDate, nameof(checkOutDate), checkInDate.AddDays(1), DateOnly.MaxValue);
        Status = status;
        TotalCost = Guard.Against.Negative(totalCost, nameof(totalCost));
    }

    public void AddRoom(ReservationRoom room)
    {
        _rooms.Add(room);
    }

    public void AddRoom(int roomId, decimal pricePerNight)
    {
        _rooms.Add(new ReservationRoom(Id, roomId, pricePerNight));
    }

    public void AddService(ReservationService service)
    {
        _services.Add(service);
    }

    public void AddService(int serviceId, int quantity, decimal unitPrice)
    {
        _services.Add(new ReservationService(Id, serviceId, quantity, unitPrice * quantity));
    }

    public void CalculateTotalCost()
    {
        var numberOfNights = (CheckOutDate.ToDateTime(TimeOnly.MinValue) - CheckInDate.ToDateTime(TimeOnly.MinValue)).Days;
        var roomsCost = _rooms.Sum(r => r.PricePerNight * numberOfNights);
        var servicesCost = _services.Sum(s => s.TotalPrice);
        TotalCost = roomsCost + servicesCost;
    }
}
