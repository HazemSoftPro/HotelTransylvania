using InnHotel.Core.SharedKernel;
using Microsoft.AspNetCore.Identity;

namespace InnHotel.Core.AuthAggregate;

public class Role : IdentityRole, IAggregateRoot
{
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    public void UpdateDetails(string name, string? description = null)
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}

public static class Roles
{
    public const string SystemAdmin = "SystemAdmin";
    public const string HotelManager = "HotelManager";
    public const string FrontDeskAgent = "FrontDeskAgent";
    public const string HousekeepingManager = "HousekeepingManager";
    public const string RestaurantManager = "RestaurantManager";
    public const string SalesManager = "SalesManager";
    public const string Guest = "Guest";
    public const string Maintenance = "Maintenance";
    public const string Accountant = "Accountant";
}
