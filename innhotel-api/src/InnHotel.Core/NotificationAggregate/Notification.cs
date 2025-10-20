namespace InnHotel.Core.NotificationAggregate;

/// <summary>
/// Represents a notification in the system
/// </summary>
public class Notification : EntityBase, IAggregateRoot
{
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public NotificationChannel Channel { get; set; }
    public NotificationStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }
    public DateTime? ReadAt { get; set; }
    
    // Related entity information
    public string? RelatedEntityType { get; set; }
    public int? RelatedEntityId { get; set; }
    
    // Delivery tracking
    public string? DeliveryId { get; set; }
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; }
    
    public Notification()
    {
    }
    
    public Notification(
        int userId,
        string title,
        string message,
        NotificationType type,
        NotificationChannel channel,
        string? relatedEntityType = null,
        int? relatedEntityId = null)
    {
        UserId = Guard.Against.NegativeOrZero(userId, nameof(userId));
        Title = Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Message = Guard.Against.NullOrWhiteSpace(message, nameof(message));
        Type = type;
        Channel = channel;
        Status = NotificationStatus.Pending;
        RelatedEntityType = relatedEntityType;
        RelatedEntityId = relatedEntityId;
    }
    
    public void MarkAsSent(string? deliveryId = null)
    {
        Status = NotificationStatus.Sent;
        SentAt = DateTime.UtcNow;
        DeliveryId = deliveryId;
    }
    
    public void MarkAsRead()
    {
        Status = NotificationStatus.Read;
        ReadAt = DateTime.UtcNow;
    }
    
    public void MarkAsFailed(string errorMessage)
    {
        Status = NotificationStatus.Failed;
        ErrorMessage = errorMessage;
        RetryCount++;
    }
}