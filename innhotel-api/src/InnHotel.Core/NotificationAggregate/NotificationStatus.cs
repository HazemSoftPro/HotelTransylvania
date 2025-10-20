namespace InnHotel.Core.NotificationAggregate;

/// <summary>
/// Status of a notification
/// </summary>
public enum NotificationStatus
{
    Pending = 1,
    Sent = 2,
    Delivered = 3,
    Read = 4,
    Failed = 5
}