namespace InnHotel.UseCases.Reservations;

public record ReservationDto
{
    public int Id { get; init; }
    public int GuestId { get; init; }
    public string GuestName { get; init; } = string.Empty;
    public DateOnly CheckInDate { get; init; }
    public DateOnly CheckOutDate { get; init; }
    public DateTime ReservationDate { get; init; }
    public ReservationStatus Status { get; init; }
    public decimal TotalCost { get; init; }
    public List<ReservationRoomDto> Rooms { get; init; } = new();
    public List<ReservationServiceDto> Services { get; init; } = new();
}

public record ReservationRoomDto
{
    public int RoomId { get; init; }
    public string RoomNumber { get; init; } = string.Empty;
    public string RoomTypeName { get; init; } = string.Empty;
    public decimal PricePerNight { get; init; }
}

public record ReservationServiceDto
{
    public int ServiceId { get; init; }
    public string ServiceName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalPrice => Quantity * UnitPrice;
}
