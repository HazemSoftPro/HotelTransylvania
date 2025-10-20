namespace InnHotel.Core.PaymentAggregate;

/// <summary>
/// Represents the status of a payment transaction
/// </summary>
public enum PaymentStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    Cancelled = 5,
    Refunded = 6,
    PartiallyRefunded = 7
}