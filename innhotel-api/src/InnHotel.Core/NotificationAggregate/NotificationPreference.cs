namespace InnHotel.Core.NotificationAggregate;

/// <summary>
/// User preferences for notifications
/// </summary>
public class NotificationPreference : EntityBase, IAggregateRoot
{
    public int UserId { get; set; }
    public NotificationType NotificationType { get; set; }
    public bool EmailEnabled { get; set; } = true;
    public bool SMSEnabled { get; set; } = false;
    public bool InAppEnabled { get; set; } = true;
    public bool PushEnabled { get; set; } = true;
    
    public NotificationPreference()
    {
    }
    
    public NotificationPreference(
        int userId,
        NotificationType notificationType,
        bool emailEnabled = true,
        bool smsEnabled = false,
        bool inAppEnabled = true,
        bool pushEnabled = true)
    {
        UserId = Guard.Against.NegativeOrZero(userId, nameof(userId));
        NotificationType = notificationType;
        EmailEnabled = emailEnabled;
        SMSEnabled = smsEnabled;
        InAppEnabled = inAppEnabled;
        PushEnabled = pushEnabled;
    }
}
