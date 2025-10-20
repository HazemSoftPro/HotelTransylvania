namespace InnHotel.Core.NotificationAggregate;

/// <summary>
/// Types of notifications in the system
/// </summary>
public enum NotificationType
{
    ReservationConfirmation = 1,
    CheckInReminder = 2,
    CheckOutReminder = 3,
    PaymentReceived = 4,
    PaymentFailed = 5,
    ReservationCancelled = 6,
    RoomReady = 7,
    MaintenanceScheduled = 8,
    SpecialOffer = 9,
    SystemAlert = 10,
    WaitlistAvailable = 11
}