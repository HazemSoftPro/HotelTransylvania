using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using InnHotel.Web.Hubs;
using InnHotel.Core.Interfaces;
using System.Collections.Concurrent;

namespace InnHotel.Web.Integration;

/// <summary>
/// Controller for providing real-time dashboard data
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IHubContext<HotelHub> _hubContext;
    private readonly ILogger<DashboardController> _logger;
    private static readonly ConcurrentDictionary<string, object> DashboardCache = new();

    public DashboardController(
        IHubContext<HotelHub> hubContext,
        ILogger<DashboardController> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Gets real-time dashboard metrics
    /// </summary>
    [HttpGet("metrics")]
    public async Task<ActionResult<DashboardMetrics>> GetMetrics()
    {
        var metrics = new DashboardMetrics
        {
            TotalRooms = await GetTotalRooms(),
            OccupiedRooms = await GetOccupiedRooms(),
            AvailableRooms = await GetAvailableRooms(),
            RoomsUnderMaintenance = await GetRoomsUnderMaintenance(),
            TodayCheckIns = await GetTodayCheckIns(),
            TodayCheckOuts = await GetTodayCheckOuts(),
            PendingReservations = await GetPendingReservations(),
            TodayRevenue = await GetTodayRevenue(),
            OccupancyRate = await CalculateOccupancyRate(),
            AverageRoomRate = await GetAverageRoomRate(),
            StaffOnDuty = await GetStaffOnDuty(),
            TasksPending = await GetTasksPending()
        };

        // Cache the metrics
        DashboardCache["metrics"] = metrics;

        return Ok(metrics);
    }

    /// <summary>
    /// Gets recent activities for the activity feed
    /// </summary>
    [HttpGet("activities")]
    public async Task<ActionResult<List<ActivityItem>>> GetRecentActivities(
        [FromQuery] int limit = 50)
    {
        var activities = await GetRecentActivitiesAsync(limit);
        return Ok(activities);
    }

    /// <summary>
    /// Gets alerts and notifications
    /// </summary>
    [HttpGet("alerts")]
    public async Task<ActionResult<List<AlertItem>>> GetAlerts()
    {
        var alerts = await GetActiveAlertsAsync();
        return Ok(alerts);
    }

    /// <summary>
    /// Gets system health status
    /// </summary>
    [HttpGet("health")]
    public async Task<ActionResult<SystemHealth>> GetSystemHealth()
    {
        var health = new SystemHealth
        {
            DatabaseStatus = await CheckDatabaseHealth(),
            ApiServiceStatus = "Healthy",
            SignalRStatus = await CheckSignalRHealth(),
            LastUpdated = DateTime.UtcNow,
            Uptime = GetUptime()
        };

        return Ok(health);
    }

    /// <summary>
    /// Forces a dashboard update to all connected clients
    /// </summary>
    [HttpPost("refresh")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> RefreshDashboard()
    {
        var metrics = new DashboardMetrics
        {
            TotalRooms = await GetTotalRooms(),
            OccupiedRooms = await GetOccupiedRooms(),
            AvailableRooms = await GetAvailableRooms(),
            RoomsUnderMaintenance = await GetRoomsUnderMaintenance(),
            TodayCheckIns = await GetTodayCheckIns(),
            TodayCheckOuts = await GetTodayCheckOuts(),
            PendingReservations = await GetPendingReservations(),
            TodayRevenue = await GetTodayRevenue(),
            OccupancyRate = await CalculateOccupancyRate(),
            AverageRoomRate = await GetAverageRoomRate(),
            StaffOnDuty = await GetStaffOnDuty(),
            TasksPending = await GetTasksPending()
        };

        await _hubContext.Clients.Group("Role_Management").SendAsync("DashboardUpdated", new { metrics });
        await _hubContext.Clients.All.SendAsync("MetricsUpdated", metrics);

        return Ok(new { Message = "Dashboard refreshed successfully" });
    }

    // Private helper methods (in a real implementation, these would query actual repositories)
    private async Task<int> GetTotalRooms() => await Task.FromResult(120);
    private async Task<int> GetOccupiedRooms() => await Task.FromResult(85);
    private async Task<int> GetAvailableRooms() => await Task.FromResult(25);
    private async Task<int> GetRoomsUnderMaintenance() => await Task.FromResult(10);
    private async Task<int> GetTodayCheckIns() => await Task.FromResult(12);
    private async Task<int> GetTodayCheckOuts() => await Task.FromResult(8);
    private async Task<int> GetPendingReservations() => await Task.FromResult(15);
    private async Task<decimal> GetTodayRevenue() => await Task.FromResult(12450.75m);
    private async Task<decimal> CalculateOccupancyRate() => await Task.FromResult(70.8m);
    private async Task<decimal> GetAverageRoomRate() => await Task.FromResult(145.50m);
    private async Task<int> GetStaffOnDuty() => await Task.FromResult(24);
    private async Task<int> GetTasksPending() => await Task.FromResult(18);

    private async Task<List<ActivityItem>> GetRecentActivitiesAsync(int limit)
    {
        return await Task.FromResult(new List<ActivityItem>
        {
            new() { Type = "CheckIn", Description = "Guest John Doe checked in to Room 101", Timestamp = DateTime.UtcNow.AddMinutes(-5) },
            new() { Type = "RoomStatus", Description = "Room 202 marked as Available", Timestamp = DateTime.UtcNow.AddMinutes(-15) },
            new() { Type = "Payment", Description = "Payment of $250.00 processed for Reservation #1234", Timestamp = DateTime.UtcNow.AddMinutes(-30) },
            new() { Type = "CheckOut", Description = "Guest Jane Smith checked out from Room 305", Timestamp = DateTime.UtcNow.AddMinutes(-45) },
            new() { Type = "Reservation", Description = "New reservation created for 3 nights", Timestamp = DateTime.UtcNow.AddMinutes(-60) }
        });
    }

    private async Task<List<AlertItem>> GetActiveAlertsAsync()
    {
        return await Task.FromResult(new List<AlertItem>
        {
            new() { Type = "Warning", Message = "Room maintenance overdue for Room 405", Priority = "High", Timestamp = DateTime.UtcNow.AddHours(-2) },
            new() { Type = "Info", Message = "High occupancy expected tomorrow (85%)", Priority = "Medium", Timestamp = DateTime.UtcNow.AddHours(-4) },
            new() { Type = "Error", Message = "Payment gateway temporarily unavailable", Priority = "High", Timestamp = DateTime.UtcNow.AddHours(-6) }
        });
    }

    private async Task<string> CheckDatabaseHealth() => await Task.FromResult("Healthy");
    private async Task<string> CheckSignalRHealth() => await Task.FromResult("Healthy");
    private TimeSpan GetUptime() => TimeSpan.FromHours(24); // Would get actual application uptime
}

// Data models
public class DashboardMetrics
{
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public int AvailableRooms { get; set; }
    public int RoomsUnderMaintenance { get; set; }
    public int TodayCheckIns { get; set; }
    public int TodayCheckOuts { get; set; }
    public int PendingReservations { get; set; }
    public decimal TodayRevenue { get; set; }
    public decimal OccupancyRate { get; set; }
    public decimal AverageRoomRate { get; set; }
    public int StaffOnDuty { get; set; }
    public int TasksPending { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

public class ActivityItem
{
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? UserId { get; set; }
}

public class AlertItem
{
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool IsResolved { get; set; }
}

public class SystemHealth
{
    public string DatabaseStatus { get; set; } = string.Empty;
    public string ApiServiceStatus { get; set; } = string.Empty;
    public string SignalRStatus { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
    public TimeSpan Uptime { get; set; }
}