namespace InnHotel.Core.ReservationAggregate;

/// <summary>
/// Represents a waitlist entry for guests waiting for room availability.
/// </summary>
public class Waitlist : EntityBase, IAggregateRoot
{
    public int GuestId { get; set; }
    public int RoomTypeId { get; set; }
    public int BranchId { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    public WaitlistStatus Status { get; set; } = WaitlistStatus.Active;
    public int Priority { get; set; } = 0;
    public string? Notes { get; set; }
    public DateTime? NotifiedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }

    public Waitlist() { }

    public Waitlist(
        int guestId,
        int roomTypeId,
        int branchId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int priority = 0,
        string? notes = null)
    {
        GuestId = Guard.Against.NegativeOrZero(guestId, nameof(guestId));
        RoomTypeId = Guard.Against.NegativeOrZero(roomTypeId, nameof(roomTypeId));
        BranchId = Guard.Against.NegativeOrZero(branchId, nameof(branchId));
        CheckInDate = checkInDate;
        CheckOutDate = Guard.Against.OutOfRange(
            checkOutDate, nameof(checkOutDate), checkInDate.AddDays(1), DateOnly.MaxValue);
        Priority = priority;
        Notes = notes;
        RequestDate = DateTime.UtcNow;
        Status = WaitlistStatus.Active;
    }

    public void MarkAsNotified()
    {
        Status = WaitlistStatus.Notified;
        NotifiedDate = DateTime.UtcNow;
        ExpiryDate = DateTime.UtcNow.AddHours(24); // 24-hour expiry after notification
    }

    public void MarkAsConverted()
    {
        Status = WaitlistStatus.Converted;
    }

    public void MarkAsExpired()
    {
        Status = WaitlistStatus.Expired;
    }

    public void MarkAsCancelled()
    {
        Status = WaitlistStatus.Cancelled;
    }
}

public enum WaitlistStatus
{
    Active,      // Waiting for availability
    Notified,    // Guest has been notified of availability
    Converted,   // Waitlist entry converted to reservation
    Expired,     // Notification expired without conversion
    Cancelled    // Guest cancelled waitlist request
}