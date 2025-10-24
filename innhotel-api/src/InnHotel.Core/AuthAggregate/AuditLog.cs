namespace InnHotel.Core.AuthAggregate;

/// <summary>
/// Represents an audit log entry for tracking sensitive operations.
/// </summary>
public class AuditLog : EntityBase, IAggregateRoot
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string IpAddress { get; set; } = string.Empty;
    public string? AdditionalInfo { get; set; }

    public AuditLog() { }

    public AuditLog(
        string userId,
        string userName,
        string action,
        string entityType,
        int? entityId = null,
        string? oldValues = null,
        string? newValues = null,
        string ipAddress = "",
        string? additionalInfo = null)
    {
        UserId = Guard.Against.NullOrEmpty(userId, nameof(userId));
        UserName = Guard.Against.NullOrEmpty(userName, nameof(userName));
        Action = Guard.Against.NullOrEmpty(action, nameof(action));
        EntityType = Guard.Against.NullOrEmpty(entityType, nameof(entityType));
        EntityId = entityId;
        OldValues = oldValues;
        NewValues = newValues;
        IpAddress = ipAddress;
        AdditionalInfo = additionalInfo;
        Timestamp = DateTime.UtcNow;
    }
}

/// <summary>
/// Defines standard audit actions.
/// </summary>
public static class AuditActions
{
    public const string Create = "Create";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string View = "View";
    public const string Login = "Login";
    public const string Logout = "Logout";
    public const string CheckIn = "CheckIn";
    public const string CheckOut = "CheckOut";
    public const string StatusChange = "StatusChange";
    public const string PaymentProcessed = "PaymentProcessed";
    public const string RoleAssigned = "RoleAssigned";
    public const string PermissionChanged = "PermissionChanged";
}