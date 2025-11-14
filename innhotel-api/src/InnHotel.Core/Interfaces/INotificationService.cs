namespace InnHotel.Core.Interfaces;

/// <summary>
/// Service for creating and managing notifications
/// </summary>
public interface INotificationService
{
    Task CreateNotificationAsync(
        string title,
        string message,
        string category,
        Dictionary<string, object>? metadata = null,
        CancellationToken cancellationToken = default,
        bool isHighPriority = false);
}