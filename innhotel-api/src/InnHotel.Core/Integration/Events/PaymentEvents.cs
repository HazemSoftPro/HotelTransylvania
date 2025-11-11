namespace InnHotel.Core.Integration.Events;

/// <summary>
/// Event fired when a payment is processed
/// </summary>
public class PaymentProcessedEvent : IntegrationEvent
{
    public int PaymentId { get; }
    public int ReservationId { get; }
    public int GuestId { get; }
    public decimal Amount { get; }
    public string PaymentMethod { get; }
    public PaymentStatus Status { get; }
    public DateTime ProcessedAt { get; }
    public string TransactionId { get; }

    public PaymentProcessedEvent(
        int paymentId,
        int reservationId,
        int guestId,
        decimal amount,
        string paymentMethod,
        PaymentStatus status,
        string transactionId) 
        : base(paymentId.ToString())
    {
        PaymentId = paymentId;
        ReservationId = reservationId;
        GuestId = guestId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Status = status;
        ProcessedAt = DateTime.UtcNow;
        TransactionId = transactionId;
    }
}

/// <summary>
/// Event fired when a payment fails
/// </summary>
public class PaymentFailedEvent : IntegrationEvent
{
    public int PaymentId { get; }
    public int ReservationId { get; }
    public int GuestId { get; }
    public decimal Amount { get; }
    public string PaymentMethod { get; }
    public string FailureReason { get; }
    public DateTime FailedAt { get; }

    public PaymentFailedEvent(
        int paymentId,
        int reservationId,
        int guestId,
        decimal amount,
        string paymentMethod,
        string failureReason) 
        : base(paymentId.ToString())
    {
        PaymentId = paymentId;
        ReservationId = reservationId;
        GuestId = guestId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        FailureReason = failureReason;
        FailedAt = DateTime.UtcNow;
    }
}

/// <summary>
/// Event fired when a refund is processed
/// </summary>
public class RefundProcessedEvent : IntegrationEvent
{
    public int RefundId { get; }
    public int PaymentId { get; }
    public int ReservationId { get; }
    public int GuestId { get; }
    public decimal RefundAmount { get; }
    public string RefundReason { get; }
    public DateTime RefundedAt { get; }

    public RefundProcessedEvent(
        int refundId,
        int paymentId,
        int reservationId,
        int guestId,
        decimal refundAmount,
        string refundReason) 
        : base(refundId.ToString())
    {
        RefundId = refundId;
        PaymentId = paymentId;
        ReservationId = reservationId;
        GuestId = guestId;
        RefundAmount = refundAmount;
        RefundReason = refundReason;
        RefundedAt = DateTime.UtcNow;
    }
}

public enum PaymentStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    Refunded = 5,
    PartiallyRefunded = 6
}