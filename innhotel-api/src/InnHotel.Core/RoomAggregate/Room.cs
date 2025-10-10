using InnHotel.Core.BranchAggregate;
using InnHotel.Core.PropertyAggregate;
using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.RoomAggregate;

public class Room : AuditableEntity, IAggregateRoot
{
    public int BranchId { get; private set; }
    public int RoomTypeId { get; private set; }
    public int? FloorId { get; private set; }
    public string RoomNumber { get; private set; }
    public RoomStatus Status { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastMaintenanceDate { get; private set; }
    public DateTime? NextMaintenanceDate { get; private set; }
    public string? MaintenanceNotes { get; private set; }

    // Navigation properties
    public Branch Branch { get; private set; } = null!;
    public RoomType RoomType { get; private set; } = null!;
    public Floor? Floor { get; private set; }
    public ICollection<RoomBlockage> Blockages { get; private set; } = new List<RoomBlockage>();

    private Room() { } // EF Core constructor

    public Room(
        int branchId,
        int roomTypeId,
        string roomNumber,
        RoomStatus status = RoomStatus.Available,
        int? floorId = null,
        string? description = null
    )
    {
        BranchId = Guard.Against.NegativeOrZero(branchId, nameof(branchId));
        RoomTypeId = Guard.Against.NegativeOrZero(roomTypeId, nameof(roomTypeId));
        RoomNumber = Guard.Against.NullOrEmpty(roomNumber, nameof(roomNumber));
        Status = status;
        FloorId = floorId;
        Description = description;
        IsActive = true;
    }

    public void UpdateStatus(RoomStatus newStatus, string? notes = null)
    {
        Status = newStatus;
        if (!string.IsNullOrEmpty(notes))
        {
            MaintenanceNotes = notes;
        }
        UpdateAuditInfo();
    }

    public void UpdateDetails(
        int roomTypeId,
        string roomNumber,
        int? floorId = null,
        string? description = null
    )
    {
        RoomTypeId = Guard.Against.NegativeOrZero(roomTypeId, nameof(roomTypeId));
        RoomNumber = Guard.Against.NullOrEmpty(roomNumber, nameof(roomNumber));
        FloorId = floorId;
        Description = description;
        UpdateAuditInfo();
    }

    public void ScheduleMaintenance(DateTime nextMaintenanceDate, string? notes = null)
    {
        NextMaintenanceDate = nextMaintenanceDate;
        MaintenanceNotes = notes;
        UpdateAuditInfo();
    }

    public void CompleteMaintenance(string? notes = null)
    {
        LastMaintenanceDate = DateTime.UtcNow;
        NextMaintenanceDate = null;
        MaintenanceNotes = notes;
        Status = RoomStatus.Available;
        UpdateAuditInfo();
    }

    public void BlockRoom(DateTime startDate, DateTime endDate, string reason, string? notes = null)
    {
        var blockage = new RoomBlockage(Id, startDate, endDate, reason, notes);
        Blockages.Add(blockage);
        UpdateAuditInfo();
    }

    public void RemoveBlockage(int blockageId)
    {
        var blockage = Blockages.FirstOrDefault(b => b.Id == blockageId);
        if (blockage != null)
        {
            blockage.Remove();
            UpdateAuditInfo();
        }
    }

    public bool IsAvailableForDate(DateTime date)
    {
        if (!IsActive || Status == RoomStatus.OutOfOrder) return false;
        
        return !Blockages.Any(b => b.IsActive && 
                                  date >= b.StartDate && 
                                  date <= b.EndDate);
    }

    public void Activate()
    {
        IsActive = true;
        UpdateAuditInfo();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateAuditInfo();
    }
}
