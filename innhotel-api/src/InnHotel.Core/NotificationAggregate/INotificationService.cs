namespace InnHotel.Core.NotificationAggregate;

/// <summary>
/// Service for sending notifications through various channels
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Sends a notification through the specified channel
    /// </summary>
    Task<NotificationResult> SendNotificationAsync(
        Notification notification, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a notification to multiple users
    /// </summary>
    Task<IEnumerable<NotificationResult>> SendBulkNotificationAsync(
        IEnumerable<Notification> notifications, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets user notification preferences
    /// </summary>
    Task<NotificationPreference?> GetUserPreferencesAsync(
        int userId, 
        NotificationType type, 
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of a notification send operation
/// </summary>
public class NotificationResult
{
    public bool Success { get; set; }
    public string? DeliveryId { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}