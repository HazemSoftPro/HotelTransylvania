using InnHotel.Core.RoomAggregate;

namespace InnHotel.Core.Integration.Events;

/// <summary>
/// Event fired when room status changes
/// </summary>
public class RoomStatusChangedEvent : IntegrationEvent
{
    public int RoomId { get; }
    public string RoomNumber { get; }
    public RoomStatus OldStatus { get; }
    public RoomStatus NewStatus { get; }
    public DateTime StatusChangedAt { get; }
    public string? ChangedBy { get; }

    public RoomStatusChangedEvent(
        int roomId, 
        string roomNumber, 
        RoomStatus oldStatus, 
        RoomStatus newStatus,
        string? changedBy = null) 
        : base(roomId.ToString())
    {
        RoomId = roomId;
        RoomNumber = roomNumber;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        StatusChangedAt = DateTime.UtcNow;
        ChangedBy = changedBy;
    }
}

/// <summary>
/// Event fired when a room is assigned to a reservation
/// </summary>
public class RoomAssignedEvent : IntegrationEvent
{
    public int RoomId { get; }
    public string RoomNumber { get; }
    public int ReservationId { get; }
    public int GuestId { get; }
    public DateOnly CheckInDate { get; }
    public DateOnly CheckOutDate { get; }

    public RoomAssignedEvent(
        int roomId, 
        string roomNumber, 
        int reservationId, 
        int guestId,
        DateOnly checkInDate,
        DateOnly checkOutDate) 
        : base(roomId.ToString())
    {
        RoomId = roomId;
        RoomNumber = roomNumber;
        ReservationId = reservationId;
        GuestId = guestId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
    }
}

/// <summary>
/// Event fired when a room becomes available
/// </summary>
public class RoomBecameAvailableEvent : IntegrationEvent
{
    public int RoomId { get; }
    public string RoomNumber { get; }
    public RoomStatus CurrentStatus { get; }
    public DateTime AvailableAt { get; }

    public RoomBecameAvailableEvent(int roomId, string roomNumber, RoomStatus currentStatus) 
        : base(roomId.ToString())
    {
        RoomId = roomId;
        RoomNumber = roomNumber;
        CurrentStatus = currentStatus;
        AvailableAt = DateTime.UtcNow;
    }
}

/// <summary>
/// Event fired when housekeeping is required for a room
/// </summary>
public class HousekeepingRequiredEvent : IntegrationEvent
{
    public int RoomId { get; }
    public string RoomNumber { get; }
    public HousekeepingPriority Priority { get; }
    public DateTime RequiredBy { get; }
    public string? SpecialInstructions { get; }

    public HousekeepingRequiredEvent(
        int roomId, 
        string roomNumber, 
        HousekeepingPriority priority,
        DateTime requiredBy,
        string? specialInstructions = null) 
        : base(roomId.ToString())
    {
        RoomId = roomId;
        RoomNumber = roomNumber;
        Priority = priority;
        RequiredBy = requiredBy;
        SpecialInstructions = specialInstructions;
    }
}

public enum HousekeepingPriority
{
    Low = 1,
    Normal = 2,
    High = 3,
    Urgent = 4
}