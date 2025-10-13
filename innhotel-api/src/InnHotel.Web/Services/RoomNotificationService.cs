using Microsoft.AspNetCore.SignalR;
using InnHotel.Web.Hubs;
using InnHotel.Core.RoomAggregate;

namespace InnHotel.Web.Services;

/// <summary>
/// Service for sending real-time room notifications via SignalR
/// </summary>
public interface IRoomNotificationService
{
    Task NotifyRoomStatusChanged(int roomId, int branchId, RoomStatus newStatus, object roomData);
    Task NotifyRoomUpdated(int roomId, int branchId, object roomData);
    Task NotifyRoomCreated(int branchId, object roomData);
    Task NotifyRoomDeleted(int roomId, int branchId);
    Task NotifyBulkRoomsUpdated(int branchId, object bulkUpdateData);
    Task NotifyMaintenanceScheduleChanged(int branchId, object maintenanceData);
    Task SendSystemNotification(int branchId, string message, string type = "info");
}

public class RoomNotificationService : IRoomNotificationService
{
    private readonly IHubContext<RoomStatusHub, IRoomStatusClient> _hubContext;
    private readonly ILogger<RoomNotificationService> _logger;

    public RoomNotificationService(
        IHubContext<RoomStatusHub, IRoomStatusClient> hubContext,
        ILogger<RoomNotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task NotifyRoomStatusChanged(int roomId, int branchId, RoomStatus newStatus, object roomData)
    {
        try
        {
            var updateData = new
            {
                RoomId = roomId,
                BranchId = branchId,
                NewStatus = newStatus,
                Timestamp = DateTime.UtcNow,
                Data = roomData
            };

            // Notify branch group
            await _hubContext.Clients.Group($"Branch_{branchId}")
                .RoomStatusChanged(updateData);

            // Notify specific room group
            await _hubContext.Clients.Group($"Room_{roomId}")
                .RoomStatusChanged(updateData);

            _logger.LogInformation("Room status change notification sent for Room {RoomId} in Branch {BranchId}", 
                roomId, branchId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send room status change notification for Room {RoomId}", roomId);
        }
    }

    public async Task NotifyRoomUpdated(int roomId, int branchId, object roomData)
    {
        try
        {
            var updateData = new
            {
                RoomId = roomId,
                BranchId = branchId,
                Timestamp = DateTime.UtcNow,
                Data = roomData
            };

            await _hubContext.Clients.Group($"Branch_{branchId}")
                .RoomUpdated(updateData);

            await _hubContext.Clients.Group($"Room_{roomId}")
                .RoomUpdated(updateData);

            _logger.LogInformation("Room update notification sent for Room {RoomId} in Branch {BranchId}", 
                roomId, branchId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send room update notification for Room {RoomId}", roomId);
        }
    }

    public async Task NotifyRoomCreated(int branchId, object roomData)
    {
        try
        {
            var createData = new
            {
                BranchId = branchId,
                Timestamp = DateTime.UtcNow,
                Data = roomData
            };

            await _hubContext.Clients.Group($"Branch_{branchId}")
                .RoomCreated(createData);

            _logger.LogInformation("Room creation notification sent for Branch {BranchId}", branchId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send room creation notification for Branch {BranchId}", branchId);
        }
    }

    public async Task NotifyRoomDeleted(int roomId, int branchId)
    {
        try
        {
            await _hubContext.Clients.Group($"Branch_{branchId}")
                .RoomDeleted(roomId);

            await _hubContext.Clients.Group($"Room_{roomId}")
                .RoomDeleted(roomId);

            _logger.LogInformation("Room deletion notification sent for Room {RoomId} in Branch {BranchId}", 
                roomId, branchId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send room deletion notification for Room {RoomId}", roomId);
        }
    }

    public async Task NotifyBulkRoomsUpdated(int branchId, object bulkUpdateData)
    {
        try
        {
            var updateData = new
            {
                BranchId = branchId,
                Timestamp = DateTime.UtcNow,
                Data = bulkUpdateData
            };

            await _hubContext.Clients.Group($"Branch_{branchId}")
                .BulkRoomsUpdated(updateData);

            _logger.LogInformation("Bulk rooms update notification sent for Branch {BranchId}", branchId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send bulk rooms update notification for Branch {BranchId}", branchId);
        }
    }

    public async Task NotifyMaintenanceScheduleChanged(int branchId, object maintenanceData)
    {
        try
        {
            var updateData = new
            {
                BranchId = branchId,
                Timestamp = DateTime.UtcNow,
                Data = maintenanceData
            };

            await _hubContext.Clients.Group($"Branch_{branchId}")
                .MaintenanceScheduleChanged(updateData);

            _logger.LogInformation("Maintenance schedule change notification sent for Branch {BranchId}", branchId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send maintenance schedule change notification for Branch {BranchId}", branchId);
        }
    }

    public async Task SendSystemNotification(int branchId, string message, string type = "info")
    {
        try
        {
            var notification = new
            {
                BranchId = branchId,
                Message = message,
                Type = type,
                Timestamp = DateTime.UtcNow
            };

            await _hubContext.Clients.Group($"Branch_{branchId}")
                .SystemNotification(notification);

            _logger.LogInformation("System notification sent to Branch {BranchId}: {Message}", branchId, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send system notification to Branch {BranchId}", branchId);
        }
    }
}
