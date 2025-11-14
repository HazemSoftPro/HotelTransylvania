using InnHotel.Core.Integration;
using InnHotel.Core.Integration.Events;
using InnHotel.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InnHotel.Web.Hubs;

/// <summary>
/// Service for sending real-time notifications through SignalR
/// </summary>
public class HotelNotificationService : INotificationService
{
    private readonly IHubContext<HotelHub> _hubContext;
    private readonly ILogger<HotelNotificationService> _logger;
    private readonly IEventBus _eventBus;

    public HotelNotificationService(
        IHubContext<HotelHub> hubContext,
        ILogger<HotelNotificationService> logger,
        IEventBus eventBus)
    {
        _hubContext = hubContext;
        _logger = logger;
        _eventBus = eventBus;
    }

    public async Task CreateNotificationAsync(
        string title,
        string message,
        string category,
        Dictionary<string, object>? metadata = null,
        CancellationToken cancellationToken = default,
        bool isHighPriority = false)
    {
        var notification = new
        {
            Id = Guid.NewGuid().ToString(),
            Title = title,
            Message = message,
            Category = category,
            Timestamp = DateTime.UtcNow,
            Metadata = metadata ?? new Dictionary<string, object>(),
            IsHighPriority = isHighPriority
        };

        // Send to all clients (will be filtered by groups on the client side)
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification, cancellationToken);

        // Send to specific department group
        await _hubContext.Clients.Group($"Dept_{category}").SendAsync("ReceiveDepartmentNotification", notification, cancellationToken);

        // Send to management for high priority notifications
        if (isHighPriority)
        {
            await _hubContext.Clients.Group("Role_Management").SendAsync("ReceiveHighPriorityNotification", notification, cancellationToken);
        }

        _logger.LogInformation("Notification sent: {Title} to category {Category}", title, category);
    }

    public async Task SendRoomUpdateAsync(int roomId, string roomNumber, RoomStatus status, string? updatedBy = null)
    {
        var roomUpdate = new
        {
            RoomId = roomId,
            RoomNumber = roomNumber,
            Status = status.ToString(),
            UpdatedBy = updatedBy,
            Timestamp = DateTime.UtcNow
        };

        await _hubContext.Clients.Group("Dept_RoomManagement").SendAsync("RoomStatusUpdated", roomUpdate);
        await _hubContext.Clients.Group("Dept_FrontDesk").SendAsync("RoomStatusUpdated", roomUpdate);
        await _hubContext.Clients.Group("Dept_Housekeeping").SendAsync("RoomStatusUpdated", roomUpdate);

        _logger.LogInformation("Room update sent: Room {RoomNumber} status changed to {Status}", roomNumber, status);
    }

    public async Task SendReservationUpdateAsync(int reservationId, ReservationStatus status, int guestId)
    {
        var reservationUpdate = new
        {
            ReservationId = reservationId,
            Status = status.ToString(),
            GuestId = guestId,
            Timestamp = DateTime.UtcNow
        };

        await _hubContext.Clients.Group("Dept_FrontDesk").SendAsync("ReservationUpdated", reservationUpdate);
        await _hubContext.Clients.Group($"User_{guestId}").SendAsync("ReservationUpdated", reservationUpdate);

        _logger.LogInformation("Reservation update sent: Reservation {ReservationId} status changed to {Status}", reservationId, status);
    }

    public async Task SendGuestUpdateAsync(int guestId, string firstName, string lastName, string updateType)
    {
        var guestUpdate = new
        {
            GuestId = guestId,
            FirstName = firstName,
            LastName = lastName,
            UpdateType = updateType,
            Timestamp = DateTime.UtcNow
        };

        await _hubContext.Clients.Group("Dept_FrontDesk").SendAsync("GuestUpdated", guestUpdate);

        _logger.LogInformation("Guest update sent: Guest {GuestId} ({FirstName} {LastName}) - {UpdateType}", 
            guestId, firstName, lastName, updateType);
    }

    public async Task SendPaymentUpdateAsync(int paymentId, int reservationId, decimal amount, PaymentStatus status)
    {
        var paymentUpdate = new
        {
            PaymentId = paymentId,
            ReservationId = reservationId,
            Amount = amount,
            Status = status.ToString(),
            Timestamp = DateTime.UtcNow
        };

        await _hubContext.Clients.Group("Dept_Accounting").SendAsync("PaymentUpdated", paymentUpdate);
        await _hubContext.Clients.Group("Dept_FrontDesk").SendAsync("PaymentUpdated", paymentUpdate);

        _logger.LogInformation("Payment update sent: Payment {PaymentId} status changed to {Status}", paymentId, status);
    }

    public async Task SendHousekeepingUpdateAsync(int roomId, string roomNumber, HousekeepingPriority priority)
    {
        var housekeepingUpdate = new
        {
            RoomId = roomId,
            RoomNumber = roomNumber,
            Priority = priority.ToString(),
            Timestamp = DateTime.UtcNow
        };

        await _hubContext.Clients.Group("Dept_Housekeeping").SendAsync("HousekeepingTaskAssigned", housekeepingUpdate);

        _logger.LogInformation("Housekeeping update sent: Room {RoomNumber} - Priority {Priority}", roomNumber, priority);
    }

    public async Task SendSystemAlertAsync(string message, string severity, Dictionary<string, object>? details = null)
    {
        var alert = new
        {
            Message = message,
            Severity = severity,
            Timestamp = DateTime.UtcNow,
            Details = details ?? new Dictionary<string, object>()
        };

        await _hubContext.Clients.All.SendAsync("SystemAlert", alert);

        _logger.LogInformation("System alert sent: {Message} - Severity: {Severity}", message, severity);
    }

    public async Task SendDashboardUpdateAsync(Dictionary<string, object> metrics)
    {
        var dashboardUpdate = new
        {
            Metrics = metrics,
            Timestamp = DateTime.UtcNow
        };

        await _hubContext.Clients.Group("Role_Management").SendAsync("DashboardUpdated", dashboardUpdate);

        _logger.LogInformation("Dashboard update sent with {Count} metrics", metrics.Count);
    }

    public async Task BroadcastIntegrationEvent(IIntegrationEvent @event)
    {
        var eventData = new
        {
            EventType = @event.EventType,
            EventId = @event.EventId,
            OccurredOn = @event.OccurredOn,
            AggregateId = @event.AggregateId,
            Metadata = @event.Metadata,
            SerializedData = JsonSerializer.Serialize(@event)
        };

        // Send to all monitoring clients
        await _hubContext.Clients.Group("Role_Admin").SendAsync("IntegrationEvent", eventData);

        _logger.LogInformation("Integration event broadcast: {EventType}", @event.EventType);
    }
}