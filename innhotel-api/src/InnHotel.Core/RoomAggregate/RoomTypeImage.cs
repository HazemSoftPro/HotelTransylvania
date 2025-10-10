using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.RoomAggregate;

/// <summary>
/// Represents images for a specific room type
/// </summary>
public class RoomTypeImage : AuditableEntity
{
    public int RoomTypeId { get; private set; }
    public string ImageUrl { get; private set; }
    public string? Caption { get; private set; }
    public bool IsPrimary { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public RoomType RoomType { get; private set; } = null!;

    private RoomTypeImage() { } // EF Core constructor

    public RoomTypeImage(int roomTypeId, string imageUrl, string? caption = null, bool isPrimary = false, int displayOrder = 0)
    {
        RoomTypeId = Guard.Against.NegativeOrZero(roomTypeId, nameof(roomTypeId));
        ImageUrl = Guard.Against.NullOrEmpty(imageUrl, nameof(imageUrl));
        Caption = caption;
        IsPrimary = isPrimary;
        DisplayOrder = displayOrder;
        IsActive = true;
    }

    public void UpdateDetails(string imageUrl, string? caption = null, int displayOrder = 0)
    {
        ImageUrl = Guard.Against.NullOrEmpty(imageUrl, nameof(imageUrl));
        Caption = caption;
        DisplayOrder = displayOrder;
        UpdateAuditInfo();
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
        UpdateAuditInfo();
    }

    public void SetAsSecondary()
    {
        IsPrimary = false;
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
