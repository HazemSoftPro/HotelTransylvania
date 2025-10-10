using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.AuthAggregate;

/// <summary>
/// Junction entity for many-to-many relationship between Roles and Permissions
/// </summary>
public class RolePermission : AuditableEntity
{
    public string RoleId { get; private set; }
    public int PermissionId { get; private set; }
    public bool IsGranted { get; private set; }

    // Navigation properties
    public Role Role { get; private set; } = null!;
    public Permission Permission { get; private set; } = null!;

    private RolePermission() { } // EF Core constructor

    public RolePermission(string roleId, int permissionId, bool isGranted = true)
    {
        RoleId = Guard.Against.NullOrEmpty(roleId, nameof(roleId));
        PermissionId = Guard.Against.NegativeOrZero(permissionId, nameof(permissionId));
        IsGranted = isGranted;
    }

    public void Grant()
    {
        IsGranted = true;
        UpdateAuditInfo();
    }

    public void Revoke()
    {
        IsGranted = false;
        UpdateAuditInfo();
    }
}
