using InnHotel.Core.GuestAggregate;

namespace InnHotel.Core.Integration.Events;

/// <summary>
/// Event fired when a new guest is created
/// </summary>
public class GuestCreatedEvent : IntegrationEvent
{
    public int GuestId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string? Email { get; }
    public string? Phone { get; }

    public GuestCreatedEvent(Guest guest) : base(guest.Id.ToString())
    {
        GuestId = guest.Id;
        FirstName = guest.FirstName;
        LastName = guest.LastName;
        Email = guest.Email;
        Phone = guest.Phone;
    }
}

/// <summary>
/// Event fired when guest information is updated
/// </summary>
public class GuestUpdatedEvent : IntegrationEvent
{
    public int GuestId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string? Email { get; }
    public string? Phone { get; }
    public string[] ChangedFields { get; }

    public GuestUpdatedEvent(Guest guest, string[] changedFields) : base(guest.Id.ToString())
    {
        GuestId = guest.Id;
        FirstName = guest.FirstName;
        LastName = guest.LastName;
        Email = guest.Email;
        Phone = guest.Phone;
        ChangedFields = changedFields;
    }
}

/// <summary>
/// Event fired when a guest checks in
/// </summary>
public class GuestCheckedInEvent : IntegrationEvent
{
    public int GuestId { get; }
    public int ReservationId { get; }
    public DateTime CheckInTime { get; }
    public int[] RoomIds { get; }

    public GuestCheckedInEvent(int guestId, int reservationId, int[] roomIds) 
        : base(guestId.ToString())
    {
        GuestId = guestId;
        ReservationId = reservationId;
        CheckInTime = DateTime.UtcNow;
        RoomIds = roomIds;
    }
}

/// <summary>
/// Event fired when a guest checks out
/// </summary>
public class GuestCheckedOutEvent : IntegrationEvent
{
    public int GuestId { get; }
    public int ReservationId { get; }
    public DateTime CheckOutTime { get; }
    public decimal TotalBill { get; }

    public GuestCheckedOutEvent(int guestId, int reservationId, decimal totalBill) 
        : base(guestId.ToString())
    {
        GuestId = guestId;
        ReservationId = reservationId;
        CheckOutTime = DateTime.UtcNow;
        TotalBill = totalBill;
    }
}