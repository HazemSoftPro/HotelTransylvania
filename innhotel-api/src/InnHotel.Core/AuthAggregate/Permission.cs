namespace InnHotel.Core.AuthAggregate;

/// <summary>
/// Represents a permission that can be assigned to roles.
/// </summary>
public class Permission : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // e.g., "Reservations", "Rooms", "Employees"
    
    private readonly List<RolePermission> _rolePermissions = new();
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    public Permission() { }

    public Permission(string name, string description, string category)
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Description = Guard.Against.NullOrEmpty(description, nameof(description));
        Category = Guard.Against.NullOrEmpty(category, nameof(category));
    }
}

/// <summary>
/// Represents the many-to-many relationship between roles and permissions.
/// </summary>
public class RolePermission : EntityBase
{
    public string RoleName { get; set; } = string.Empty;
    public int PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;

    public RolePermission() { }

    public RolePermission(string roleName, int permissionId)
    {
        RoleName = Guard.Against.NullOrEmpty(roleName, nameof(roleName));
        PermissionId = Guard.Against.NegativeOrZero(permissionId, nameof(permissionId));
    }
}

/// <summary>
/// Defines standard permissions for the system.
/// </summary>
public static class Permissions
{
    // Reservation Permissions
    public const string ViewReservations = "Reservations.View";
    public const string CreateReservations = "Reservations.Create";
    public const string UpdateReservations = "Reservations.Update";
    public const string DeleteReservations = "Reservations.Delete";
    public const string CheckInGuests = "Reservations.CheckIn";
    public const string CheckOutGuests = "Reservations.CheckOut";

    // Room Permissions
    public const string ViewRooms = "Rooms.View";
    public const string CreateRooms = "Rooms.Create";
    public const string UpdateRooms = "Rooms.Update";
    public const string DeleteRooms = "Rooms.Delete";
    public const string UpdateRoomStatus = "Rooms.UpdateStatus";

    // Guest Permissions
    public const string ViewGuests = "Guests.View";
    public const string CreateGuests = "Guests.Create";
    public const string UpdateGuests = "Guests.Update";
    public const string DeleteGuests = "Guests.Delete";

    // Employee Permissions
    public const string ViewEmployees = "Employees.View";
    public const string CreateEmployees = "Employees.Create";
    public const string UpdateEmployees = "Employees.Update";
    public const string DeleteEmployees = "Employees.Delete";
    public const string ManageRoles = "Employees.ManageRoles";

    // Financial Permissions
    public const string ViewFinancialReports = "Financial.ViewReports";
    public const string ProcessPayments = "Financial.ProcessPayments";
    public const string ViewRevenue = "Financial.ViewRevenue";

    // System Permissions
    public const string ViewAuditLogs = "System.ViewAuditLogs";
    public const string ManageSettings = "System.ManageSettings";
    public const string ManageBranches = "System.ManageBranches";
}