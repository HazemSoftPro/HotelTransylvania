using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.RoomAggregate;

/// <summary>
/// Represents a room blockage for maintenance or other reasons
/// </summary>
public class RoomBlockage : AuditableEntity
{
    public int RoomId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string Reason { get; private set; }
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public Room Room { get; private set; } = null!;

    private RoomBlockage() { } // EF Core constructor

    public RoomBlockage(int roomId, DateTime startDate, DateTime endDate, string reason, string? notes = null)
    {
        RoomId = Guard.Against.NegativeOrZero(roomId, nameof(roomId));
        StartDate = startDate;
        EndDate = Guard.Against.OutOfRange(endDate, nameof(endDate), startDate, DateTime.MaxValue);
        Reason = Guard.Against.NullOrEmpty(reason, nameof(reason));
        Notes = notes;
        IsActive = true;
    }

    public void UpdateDetails(DateTime startDate, DateTime endDate, string reason, string? notes = null)
    {
        StartDate = startDate;
        EndDate = Guard.Against.OutOfRange(endDate, nameof(endDate), startDate, DateTime.MaxValue);
        Reason = Guard.Against.NullOrEmpty(reason, nameof(reason));
        Notes = notes;
        UpdateAuditInfo();
    }

    public void Remove()
    {
        IsActive = false;
        UpdateAuditInfo();
    }

    public bool IsActiveForDate(DateTime date)
    {
        return IsActive && date >= StartDate && date <= EndDate;
    }
}

public static class BlockageReasons
{
    public const string Maintenance = "Maintenance";
    public const string Renovation = "Renovation";
    public const string OutOfOrder = "OutOfOrder";
    public const string StaffUse = "StaffUse";
    public const string VIPReserved = "VIPReserved";
    public const string Other = "Other";
}
