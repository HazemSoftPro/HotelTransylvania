using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.AuthAggregate;

/// <summary>
/// Represents a specific permission in the system
/// </summary>
public class Permission : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string DisplayName { get; private set; }
    public string Description { get; private set; }
    public string Module { get; private set; }
    public string Action { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

    private Permission() { } // EF Core constructor

    public Permission(string name, string displayName, string description, string module, string action)
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        DisplayName = Guard.Against.NullOrEmpty(displayName, nameof(displayName));
        Description = Guard.Against.NullOrEmpty(description, nameof(description));
        Module = Guard.Against.NullOrEmpty(module, nameof(module));
        Action = Guard.Against.NullOrEmpty(action, nameof(action));
        IsActive = true;
    }

    public void UpdateDetails(string displayName, string description, string module, string action)
    {
        DisplayName = Guard.Against.NullOrEmpty(displayName, nameof(displayName));
        Description = Guard.Against.NullOrEmpty(description, nameof(description));
        Module = Guard.Against.NullOrEmpty(module, nameof(module));
        Action = Guard.Against.NullOrEmpty(action, nameof(action));
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
