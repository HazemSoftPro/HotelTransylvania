using InnHotel.Core.Interfaces;
using InnHotel.Core.NotificationAggregate;
using Microsoft.Extensions.Logging;

namespace InnHotel.Infrastructure.Notifications;

/// <summary>
/// Service for sending notifications through various channels
/// </summary>
public class NotificationService : INotificationService
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<NotificationService> _logger;
    private readonly IReadRepository<NotificationPreference> _preferenceRepository;

    public NotificationService(
        IEmailSender emailSender,
        ILogger<NotificationService> logger,
        IReadRepository<NotificationPreference> preferenceRepository)
    {
        _emailSender = emailSender;
        _logger = logger;
        _preferenceRepository = preferenceRepository;
    }
    public async Task<NotificationResult> SendNotificationAsync(
        Notification notification,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation(
                "Sending notification to user {UserId} via {Channel}. Type: {Type}",
                notification.UserId, notification.Channel, notification.Type);

            // Check user preferences
            var preferences = await GetUserPreferencesAsync(
                notification.UserId, 
                notification.Type, 
                cancellationToken);

            if (preferences != null && !IsChannelEnabled(preferences, notification.Channel))
            {
                _logger.LogInformation(
                    "Notification skipped - channel {Channel} disabled for user {UserId}",
                    notification.Channel, notification.UserId);

                return new NotificationResult
                {
                    Success = false,
                    ErrorMessage = "Channel disabled by user preferences"
                };
            }

            // Send based on channel
            switch (notification.Channel)
            {
                case NotificationChannel.Email:
                    return await SendEmailNotificationAsync(notification, cancellationToken);
                
                case NotificationChannel.InApp:
                    return await SendInAppNotificationAsync(notification, cancellationToken);
                
                case NotificationChannel.SMS:
                    return await SendSMSNotificationAsync(notification, cancellationToken);
                
                case NotificationChannel.Push:
                    return await SendPushNotificationAsync(notification, cancellationToken);
                
                default:
                    throw new NotSupportedException($"Channel {notification.Channel} is not supported");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Error sending notification to user {UserId}", 
                notification.UserId);

            return new NotificationResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<IEnumerable<NotificationResult>> SendBulkNotificationAsync(
        IEnumerable<Notification> notifications,
        CancellationToken cancellationToken = default)
    {
        var results = new List<NotificationResult>();

        foreach (var notification in notifications)
        {
            var result = await SendNotificationAsync(notification, cancellationToken);
            results.Add(result);
        }

        return results;
    }

    public async Task<NotificationPreference?> GetUserPreferencesAsync(
        int userId,
        NotificationType type,
        CancellationToken cancellationToken = default)
    {
        var preferences = await _preferenceRepository.ListAsync(cancellationToken);
        return preferences.FirstOrDefault(p => p.UserId == userId && p.NotificationType == type);
    }

    private async Task<NotificationResult> SendEmailNotificationAsync(
        Notification notification,
        CancellationToken cancellationToken)
    {
        try
        {
            // In production, get user email from user service
            var userEmail = $"user{notification.UserId}@innhotel.com";
            
            await _emailSender.SendEmailAsync(
                userEmail,
                "noreply@innhotel.com",
                notification.Title,
                notification.Message);

            return new NotificationResult
            {
                Success = true,
                DeliveryId = Guid.NewGuid().ToString()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email notification");
            return new NotificationResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    private async Task<NotificationResult> SendInAppNotificationAsync(
        Notification notification,
        CancellationToken cancellationToken)
    {
        // In-app notifications are stored in database and retrieved by client
        // SignalR can be used to push real-time updates
        await Task.CompletedTask;

        return new NotificationResult
        {
            Success = true,
            DeliveryId = Guid.NewGuid().ToString()
        };
    }

    private async Task<NotificationResult> SendSMSNotificationAsync(
        Notification notification,
        CancellationToken cancellationToken)
    {
        // Mock SMS sending - integrate with Twilio or similar service in production
        await Task.Delay(100, cancellationToken);

        _logger.LogInformation(
            "SMS notification sent to user {UserId}: {Message}",
            notification.UserId, notification.Message);

        return new NotificationResult
        {
            Success = true,
            DeliveryId = $"sms_{Guid.NewGuid():N}"
        };
    }

    private async Task<NotificationResult> SendPushNotificationAsync(
        Notification notification,
        CancellationToken cancellationToken)
    {
        // Mock push notification - integrate with Firebase or similar service in production
        await Task.Delay(100, cancellationToken);

        _logger.LogInformation(
            "Push notification sent to user {UserId}: {Message}",
            notification.UserId, notification.Message);

        return new NotificationResult
        {
            Success = true,
            DeliveryId = $"push_{Guid.NewGuid():N}"
        };
    }

    private bool IsChannelEnabled(NotificationPreference preferences, NotificationChannel channel)
    {
        return channel switch
        {
            NotificationChannel.Email => preferences.EmailEnabled,
            NotificationChannel.SMS => preferences.SMSEnabled,
            NotificationChannel.InApp => preferences.InAppEnabled,
            NotificationChannel.Push => preferences.PushEnabled,
            _ => false
        };
    }
}