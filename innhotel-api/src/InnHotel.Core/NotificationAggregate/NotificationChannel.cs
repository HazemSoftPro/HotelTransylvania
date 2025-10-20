namespace InnHotel.Core.NotificationAggregate;

/// <summary>
/// Channels through which notifications can be delivered
/// </summary>
public enum NotificationChannel
{
    InApp = 1,
    Email = 2,
    SMS = 3,
    Push = 4
}