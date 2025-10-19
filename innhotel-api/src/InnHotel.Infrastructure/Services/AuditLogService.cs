using InnHotel.Core.AuthAggregate;
using InnHotel.Core.Interfaces;

namespace InnHotel.Infrastructure.Services;

/// <summary>
/// Implementation of audit logging service.
/// </summary>
public class AuditLogService(IRepository<AuditLog> auditLogRepository) : IAuditLogService
{
    public async Task LogAsync(
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
        var auditLog = new AuditLog(
            userId,
            userName,
            action,
            entityType,
            entityId,
            oldValues,
            newValues,
            ipAddress,
            additionalInfo);

        await auditLogRepository.AddAsync(auditLog);
    }

    public async Task<IEnumerable<AuditLog>> GetLogsAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? userId = null,
        string? action = null,
        string? entityType = null,
        int pageNumber = 1,
        int pageSize = 50)
    {
        var logs = await auditLogRepository.ListAsync();

        // Apply filters
        if (startDate.HasValue)
            logs = logs.Where(l => l.Timestamp >= startDate.Value).ToList();

        if (endDate.HasValue)
            logs = logs.Where(l => l.Timestamp <= endDate.Value).ToList();

        if (!string.IsNullOrEmpty(userId))
            logs = logs.Where(l => l.UserId == userId).ToList();

        if (!string.IsNullOrEmpty(action))
            logs = logs.Where(l => l.Action == action).ToList();

        if (!string.IsNullOrEmpty(entityType))
            logs = logs.Where(l => l.EntityType == entityType).ToList();

        // Apply pagination
        return logs
            .OrderByDescending(l => l.Timestamp)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}