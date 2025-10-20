using InnHotel.Core.NotificationAggregate;

namespace InnHotel.Infrastructure.Notifications;

/// <summary>
/// Templates for different notification types
/// </summary>
public static class NotificationTemplates
{
    public static (string Title, string Message) GetReservationConfirmation(
        string guestName, 
        string roomNumber, 
        DateTime checkInDate, 
        DateTime checkOutDate)
    {
        var title = "Reservation Confirmed";
        var message = $@"Dear {guestName},

Your reservation has been confirmed!

Room: {roomNumber}
Check-in: {checkInDate:MMM dd, yyyy}
Check-out: {checkOutDate:MMM dd, yyyy}

We look forward to welcoming you to InnHotel.

Best regards,
InnHotel Team";

        return (title, message);
    }

    public static (string Title, string Message) GetCheckInReminder(
        string guestName, 
        string roomNumber, 
        DateTime checkInDate)
    {
        var title = "Check-In Reminder";
        var message = $@"Dear {guestName},

This is a friendly reminder that your check-in is scheduled for {checkInDate:MMM dd, yyyy}.

Room: {roomNumber}
Check-in time: 3:00 PM

Please ensure you have a valid ID for check-in.

See you soon!
InnHotel Team";

        return (title, message);
    }

    public static (string Title, string Message) GetCheckOutReminder(
        string guestName, 
        DateTime checkOutDate)
    {
        var title = "Check-Out Reminder";
        var message = $@"Dear {guestName},

Your check-out is scheduled for {checkOutDate:MMM dd, yyyy}.

Check-out time: 11:00 AM

Please ensure all personal belongings are collected and room keys are returned at the front desk.

Thank you for staying with us!
InnHotel Team";

        return (title, message);
    }

    public static (string Title, string Message) GetPaymentReceived(
        string guestName, 
        decimal amount, 
        string transactionId)
    {
        var title = "Payment Received";
        var message = $@"Dear {guestName},

We have received your payment of ${amount:F2}.

Transaction ID: {transactionId}

Thank you for your payment.

InnHotel Team";

        return (title, message);
    }

    public static (string Title, string Message) GetPaymentFailed(
        string guestName, 
        decimal amount, 
        string reason)
    {
        var title = "Payment Failed";
        var message = $@"Dear {guestName},

We were unable to process your payment of ${amount:F2}.

Reason: {reason}

Please contact our front desk or try again with a different payment method.

InnHotel Team";

        return (title, message);
    }

    public static (string Title, string Message) GetReservationCancelled(
        string guestName, 
        string roomNumber, 
        DateTime checkInDate)
    {
        var title = "Reservation Cancelled";
        var message = $@"Dear {guestName},

Your reservation has been cancelled.

Room: {roomNumber}
Original Check-in: {checkInDate:MMM dd, yyyy}

If this was a mistake, please contact us immediately.

InnHotel Team";

        return (title, message);
    }

    public static (string Title, string Message) GetWaitlistAvailable(
        string guestName, 
        string roomType, 
        DateTime checkInDate)
    {
        var title = "Room Available from Waitlist";
        var message = $@"Dear {guestName},

Good news! A {roomType} room is now available for your requested dates.

Check-in: {checkInDate:MMM dd, yyyy}

Please contact us within 24 hours to confirm your reservation.

InnHotel Team";

        return (title, message);
    }
}