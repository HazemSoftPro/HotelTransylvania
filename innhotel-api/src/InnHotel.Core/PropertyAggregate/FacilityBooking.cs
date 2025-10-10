using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.PropertyAggregate;

/// <summary>
/// Represents a booking for hotel facilities (conference hall, spa, etc.)
/// </summary>
public class FacilityBooking : AuditableEntity
{
    public int FacilityId { get; private set; }
    public int? GuestId { get; private set; }
    public string GuestName { get; private set; }
    public string? GuestEmail { get; private set; }
    public string? GuestPhone { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; }
    public string? Notes { get; private set; }
    public string? CancellationReason { get; private set; }

    // Navigation properties
    public Facility Facility { get; private set; } = null!;

    private FacilityBooking() { } // EF Core constructor

    public FacilityBooking(
        int facilityId,
        string guestName,
        DateTime startDateTime,
        DateTime endDateTime,
        decimal totalAmount,
        int? guestId = null,
        string? guestEmail = null,
        string? guestPhone = null,
        string? notes = null
    )
    {
        FacilityId = Guard.Against.NegativeOrZero(facilityId, nameof(facilityId));
        GuestName = Guard.Against.NullOrEmpty(guestName, nameof(guestName));
        StartDateTime = startDateTime;
        EndDateTime = Guard.Against.OutOfRange(endDateTime, nameof(endDateTime), startDateTime, DateTime.MaxValue);
        TotalAmount = Guard.Against.Negative(totalAmount, nameof(totalAmount));
        GuestId = guestId;
        GuestEmail = guestEmail;
        GuestPhone = guestPhone;
        Notes = notes;
        Status = FacilityBookingStatus.Confirmed;
    }

    public void UpdateDetails(
        string guestName,
        DateTime startDateTime,
        DateTime endDateTime,
        decimal totalAmount,
        string? guestEmail = null,
        string? guestPhone = null,
        string? notes = null
    )
    {
        GuestName = Guard.Against.NullOrEmpty(guestName, nameof(guestName));
        StartDateTime = startDateTime;
        EndDateTime = Guard.Against.OutOfRange(endDateTime, nameof(endDateTime), startDateTime, DateTime.MaxValue);
        TotalAmount = Guard.Against.Negative(totalAmount, nameof(totalAmount));
        GuestEmail = guestEmail;
        GuestPhone = guestPhone;
        Notes = notes;
        UpdateAuditInfo();
    }

    public void Confirm()
    {
        Status = FacilityBookingStatus.Confirmed;
        UpdateAuditInfo();
    }

    public void Cancel(string reason)
    {
        Status = FacilityBookingStatus.Cancelled;
        CancellationReason = reason;
        UpdateAuditInfo();
    }

    public void Complete()
    {
        Status = FacilityBookingStatus.Completed;
        UpdateAuditInfo();
    }

    public void NoShow()
    {
        Status = FacilityBookingStatus.NoShow;
        UpdateAuditInfo();
    }
}

public static class FacilityBookingStatus
{
    public const string Pending = "Pending";
    public const string Confirmed = "Confirmed";
    public const string InProgress = "InProgress";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";
    public const string NoShow = "NoShow";
}
