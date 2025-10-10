using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.GuestAggregate;

/// <summary>
/// Represents a guest preference (e.g., Non-smoking room, King bed, Pool view)
/// </summary>
public class GuestPreference : AuditableEntity
{
    public int GuestId { get; private set; }
    public string Type { get; private set; }
    public string Value { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public Guest Guest { get; private set; } = null!;

    private GuestPreference() { } // EF Core constructor

    public GuestPreference(int guestId, string type, string value, string? description = null)
    {
        GuestId = Guard.Against.NegativeOrZero(guestId, nameof(guestId));
        Type = Guard.Against.NullOrEmpty(type, nameof(type));
        Value = Guard.Against.NullOrEmpty(value, nameof(value));
        Description = description;
        IsActive = true;
    }

    public void UpdateDetails(string type, string value, string? description = null)
    {
        Type = Guard.Against.NullOrEmpty(type, nameof(type));
        Value = Guard.Against.NullOrEmpty(value, nameof(value));
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
