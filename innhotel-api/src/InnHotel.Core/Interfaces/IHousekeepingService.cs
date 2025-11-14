namespace InnHotel.Core.Interfaces;

/// <summary>
/// Service for managing housekeeping operations
/// </summary>
public interface IHousekeepingService
{
    Task MarkRoomAsOccupiedAsync(int roomId, CancellationToken cancellationToken = default);
    Task ScheduleCleaningAsync(int reservationId, CancellationToken cancellationToken = default);
    Task ScheduleImmediateCleaningAsync(int roomId, CancellationToken cancellationToken = default);
    Task<HousekeepingAssignment> AssignHousekeepingStaffAsync(
        int roomId, 
        HousekeepingPriority priority, 
        DateTime requiredBy, 
        CancellationToken cancellationToken = default);
    Task UpdateRoomOccupancyAsync(int roomId, bool isOccupied, CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents a housekeeping assignment
/// </summary>
public class HousekeepingAssignment
{
    public int? StaffId { get; set; }
    public string StaffName { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
    public DateTime DueBy { get; set; }
    public HousekeepingPriority Priority { get; set; }
}

/// <summary>
/// Housekeeping priority levels
/// </summary>
public enum HousekeepingPriority
{
    Low = 1,
    Normal = 2,
    High = 3,
    Urgent = 4
}