using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using InnHotel.Core.AuthAggregate;

namespace InnHotel.Web.Hubs;

/// <summary>
/// SignalR Hub for real-time room status updates
/// </summary>
[Authorize]
public class RoomStatusHub : Hub
{
    private readonly ILogger<RoomStatusHub> _logger;

    public RoomStatusHub(ILogger<RoomStatusHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Join a branch group to receive updates for specific branch
    /// </summary>
    /// <param name="branchId">Branch ID to join</param>
    public async Task JoinBranchGroup(string branchId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Branch_{branchId}");
        _logger.LogInformation("User {UserId} joined branch group {BranchId}", 
            Context.UserIdentifier, branchId);
    }

    /// <summary>
    /// Leave a branch group
    /// </summary>
    /// <param name="branchId">Branch ID to leave</param>
    public async Task LeaveBranchGroup(string branchId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Branch_{branchId}");
        _logger.LogInformation("User {UserId} left branch group {BranchId}", 
            Context.UserIdentifier, branchId);
    }

    /// <summary>
    /// Join room-specific group for detailed updates
    /// </summary>
    /// <param name="roomId">Room ID to monitor</param>
    public async Task JoinRoomGroup(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Room_{roomId}");
        _logger.LogInformation("User {UserId} joined room group {RoomId}", 
            Context.UserIdentifier, roomId);
    }

    /// <summary>
    /// Leave room-specific group
    /// </summary>
    /// <param name="roomId">Room ID to stop monitoring</param>
    public async Task LeaveRoomGroup(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Room_{roomId}");
        _logger.LogInformation("User {UserId} left room group {RoomId}", 
            Context.UserIdentifier, roomId);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("User {UserId} connected to RoomStatusHub", 
            Context.UserIdentifier);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("User {UserId} disconnected from RoomStatusHub. Exception: {Exception}", 
            Context.UserIdentifier, exception?.Message);
        await base.OnDisconnectedAsync(exception);
    }
}

/// <summary>
/// Interface for strongly-typed SignalR client methods
/// </summary>
public interface IRoomStatusClient
{
    /// <summary>
    /// Notify clients of room status change
    /// </summary>
    Task RoomStatusChanged(object roomUpdate);

    /// <summary>
    /// Notify clients of room details update
    /// </summary>
    Task RoomUpdated(object roomUpdate);

    /// <summary>
    /// Notify clients of new room creation
    /// </summary>
    Task RoomCreated(object roomData);

    /// <summary>
    /// Notify clients of room deletion
    /// </summary>
    Task RoomDeleted(int roomId);

    /// <summary>
    /// Notify clients of bulk room updates
    /// </summary>
    Task BulkRoomsUpdated(object bulkUpdateData);

    /// <summary>
    /// Notify clients of maintenance schedule changes
    /// </summary>
    Task MaintenanceScheduleChanged(object maintenanceData);

    /// <summary>
    /// Send system notifications
    /// </summary>
    Task SystemNotification(object notification);
}
