using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.RoomAggregate;

/// <summary>
/// Represents amenities available for a specific room type
/// </summary>
public class RoomTypeAmenity : AuditableEntity
{
    public int RoomTypeId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public RoomType RoomType { get; private set; } = null!;

    private RoomTypeAmenity() { } // EF Core constructor

    public RoomTypeAmenity(int roomTypeId, string name, string? description = null)
    {
        RoomTypeId = Guard.Against.NegativeOrZero(roomTypeId, nameof(roomTypeId));
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Description = description;
        IsActive = true;
    }

    public void UpdateDetails(string name, string? description = null)
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Description = description;
        UpdateAuditInfo();
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
