namespace InnHotel.Core.Integration.Events;

/// <summary>
/// Event fired when a new reservation is created
/// </summary>
public class ReservationCreatedEvent : IntegrationEvent
{
    public int ReservationId { get; }
    public int GuestId { get; }
    public DateOnly CheckInDate { get; }
    public DateOnly CheckOutDate { get; }
    public decimal TotalCost { get; }
    public int[] RoomIds { get; }
    public int[] ServiceIds { get; }

    public ReservationCreatedEvent(
        int reservationId,
        int guestId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        decimal totalCost,
        int[] roomIds,
        int[] serviceIds) 
        : base(reservationId.ToString())
    {
        ReservationId = reservationId;
        GuestId = guestId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        TotalCost = totalCost;
        RoomIds = roomIds;
        ServiceIds = serviceIds;
    }
}

/// <summary>
/// Event fired when a reservation is modified
/// </summary>
public class ReservationModifiedEvent : IntegrationEvent
{
    public int ReservationId { get; }
    public int GuestId { get; }
    public DateOnly OldCheckInDate { get; }
    public DateOnly OldCheckOutDate { get; }
    public DateOnly NewCheckInDate { get; }
    public DateOnly NewCheckOutDate { get; }
    public decimal OldTotalCost { get; }
    public decimal NewTotalCost { get; }
    public string[] ChangedFields { get; }

    public ReservationModifiedEvent(
        int reservationId,
        int guestId,
        DateOnly oldCheckInDate,
        DateOnly oldCheckOutDate,
        DateOnly newCheckInDate,
        DateOnly newCheckOutDate,
        decimal oldTotalCost,
        decimal newTotalCost,
        string[] changedFields) 
        : base(reservationId.ToString())
    {
        ReservationId = reservationId;
        GuestId = guestId;
        OldCheckInDate = oldCheckInDate;
        OldCheckOutDate = oldCheckOutDate;
        NewCheckInDate = newCheckInDate;
        NewCheckOutDate = newCheckOutDate;
        OldTotalCost = oldTotalCost;
        NewTotalCost = newTotalCost;
        ChangedFields = changedFields;
    }
}

/// <summary>
/// Event fired when a reservation is cancelled
/// </summary>
public class ReservationCancelledEvent : IntegrationEvent
{
    public int ReservationId { get; }
    public int GuestId { get; }
    public DateOnly CheckInDate { get; }
    public DateOnly CheckOutDate { get; }
    public decimal RefundAmount { get; }
    public string CancellationReason { get; }
    public int[] ReleasedRoomIds { get; }

    public ReservationCancelledEvent(
        int reservationId,
        int guestId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        decimal refundAmount,
        string cancellationReason,
        int[] releasedRoomIds) 
        : base(reservationId.ToString())
    {
        ReservationId = reservationId;
        GuestId = guestId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        RefundAmount = refundAmount;
        CancellationReason = cancellationReason;
        ReleasedRoomIds = releasedRoomIds;
    }
}

/// <summary>
/// Event fired when a reservation is confirmed
/// </summary>
public class ReservationConfirmedEvent : IntegrationEvent
{
    public int ReservationId { get; }
    public int GuestId { get; }
    public DateOnly CheckInDate { get; }
    public DateOnly CheckOutDate { get; }
    public decimal TotalCost { get; }
    public DateTime ConfirmedAt { get; }

    public ReservationConfirmedEvent(
        int reservationId,
        int guestId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        decimal totalCost) 
        : base(reservationId.ToString())
    {
        ReservationId = reservationId;
        GuestId = guestId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        TotalCost = totalCost;
        ConfirmedAt = DateTime.UtcNow;
    }
}