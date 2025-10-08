namespace InnHotel.Web.Reservations;

/// <summary>
/// Represents a reservation response record
/// </summary>
public record ReservationRecord(
    int Id,
    int GuestId,
    string GuestName,
    DateOnly CheckInDate,
    DateOnly CheckOutDate,
    DateTime ReservationDate,
    ReservationStatus Status,
    decimal TotalCost,
    List<ReservationRoomRecord> Rooms,
    List<ReservationServiceRecord> Services
);

/// <summary>
/// Represents a room in a reservation
/// </summary>
public record ReservationRoomRecord(
    int RoomId,
    string RoomNumber,
    string RoomTypeName,
    decimal PricePerNight
);

/// <summary>
/// Represents a service in a reservation
/// </summary>
public record ReservationServiceRecord(
    int ServiceId,
    string ServiceName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);
