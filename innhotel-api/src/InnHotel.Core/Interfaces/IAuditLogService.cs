using InnHotel.Core.AuthAggregate;

namespace InnHotel.Core.Interfaces;

/// <summary>
/// Service interface for audit logging operations.
/// </summary>
public interface IAuditLogService
{
    /// <summary>
    /// Logs an audit entry.
    /// </summary>
    Task LogAsync(
        string userId,
        string userName,
        string action,
        string entityType,
        int? entityId = null,
        string? oldValues = null,
        string? newValues = null,
        string ipAddress = "",
        string? additionalInfo = null);

    /// <summary>
    /// Gets audit logs with filtering.
    /// </summary>
    Task<IEnumerable<AuditLog>> GetLogsAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? userId = null,
        string? action = null,
        string? entityType = null,
        int pageNumber = 1,
        int pageSize = 50);
}