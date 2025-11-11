using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace InnHotel.Web.Hubs;

/// <summary>
/// SignalR hub for real-time hotel management updates
/// </summary>
[Authorize]
public class HotelHub : Hub
{
    private readonly ILogger<HotelHub> _logger;
    private static readonly Dictionary<string, HashSet<string>> UserGroups = new();
    private static readonly Dictionary<string, string> ConnectionUsers = new();

    public HotelHub(ILogger<HotelHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        var connectionId = Context.ConnectionId;

        if (userId != null)
        {
            // Track user connection
            ConnectionUsers[connectionId] = userId;

            if (!UserGroups.ContainsKey(userId))
            {
                UserGroups[userId] = new HashSet<string>();
            }
            UserGroups[userId].Add(connectionId);

            // Add to role-based groups
            var userRoles = Context.User?.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();

            foreach (var role in userRoles)
            {
                await Groups.AddToGroupAsync(connectionId, $"Role_{role}");
                _logger.LogInformation("Added user {UserId} to role group {Role}", userId, role);
            }

            // Add to user-specific group
            await Groups.AddToGroupAsync(connectionId, $"User_{userId}");

            _logger.LogInformation("User {UserId} connected with connection ID {ConnectionId}", userId, connectionId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        var connectionId = Context.ConnectionId;

        if (userId != null && UserGroups.ContainsKey(userId))
        {
            UserGroups[userId].Remove(connectionId);
            ConnectionUsers.Remove(connectionId);

            // Remove from role-based groups
            var userRoles = Context.User?.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();

            foreach (var role in userRoles)
            {
                await Groups.RemoveFromGroupAsync(connectionId, $"Role_{role}");
            }

            // Remove from user-specific group
            await Groups.RemoveFromGroupAsync(connectionId, $"User_{userId}");

            // Clean up empty user group
            if (!UserGroups[userId].Any())
            {
                UserGroups.Remove(userId);
            }

            _logger.LogInformation("User {UserId} disconnected with connection ID {ConnectionId}", userId, connectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Join a department-specific group for targeted notifications
    /// </summary>
    public async Task JoinDepartmentGroup(string department)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Dept_{department}");
            _logger.LogInformation("User {UserId} joined department group {Department}", userId, department);
        }
    }

    /// <summary>
    /// Leave a department-specific group
    /// </summary>
    public async Task LeaveDepartmentGroup(string department)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Dept_{department}");
            _logger.LogInformation("User {UserId} left department group {Department}", userId, department);
        }
    }

    /// <summary>
    /// Join a branch-specific group for multi-property support
    /// </summary>
    public async Task JoinBranchGroup(int branchId)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Branch_{branchId}");
            _logger.LogInformation("User {UserId} joined branch group {BranchId}", userId, branchId);
        }
    }

    /// <summary>
    /// Leave a branch-specific group
    /// </summary>
    public async Task LeaveBranchGroup(int branchId)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Branch_{branchId}");
            _logger.LogInformation("User {UserId} left branch group {BranchId}", userId, branchId);
        }
    }

    /// <summary>
    /// Get current online users count
    /// </summary>
    public int GetOnlineUsersCount()
    {
        return ConnectionUsers.Distinct().Count();
    }

    /// <summary>
    /// Get online users in a specific role
    /// </summary>
    public async Task<List<string>> GetOnlineUsersInRoleAsync(string role)
    {
        // This would need to be implemented with a proper user service
        // For now, return empty list
        return new List<string>();
    }
}