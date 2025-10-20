using InnHotel.Core.ReservationAggregate;

namespace InnHotel.Core.PaymentAggregate;

/// <summary>
/// Represents a payment transaction in the hotel management system
/// </summary>
public class Payment : EntityBase, IAggregateRoot
{
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;
    
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedDate { get; set; }
    
    // Payment provider details
    public string? TransactionId { get; set; }
    public string? PaymentProvider { get; set; }
    public string? PaymentProviderResponse { get; set; }
    
    // Refund information
    public bool IsRefunded { get; set; }
    public decimal RefundedAmount { get; set; }
    public DateTime? RefundDate { get; set; }
    public string? RefundReason { get; set; }
    
    // Additional charges
    public string? Description { get; set; }
    public string? Notes { get; set; }
    
    // Audit fields
    public int ProcessedByUserId { get; set; }
    
    public Payment()
    {
    }
    
    public Payment(
        int reservationId,
        decimal amount,
        PaymentMethod method,
        PaymentStatus status,
        int processedByUserId,
        string? description = null)
    {
        ReservationId = Guard.Against.NegativeOrZero(reservationId, nameof(reservationId));
        Amount = Guard.Against.Negative(amount, nameof(amount));
        Method = method;
        Status = status;
        ProcessedByUserId = Guard.Against.NegativeOrZero(processedByUserId, nameof(processedByUserId));
        Description = description;
    }
    
    /// <summary>
    /// Marks the payment as processed successfully
    /// </summary>
    public void MarkAsProcessed(string transactionId, string provider)
    {
        Status = PaymentStatus.Completed;
        ProcessedDate = DateTime.UtcNow;
        TransactionId = Guard.Against.NullOrWhiteSpace(transactionId, nameof(transactionId));
        PaymentProvider = Guard.Against.NullOrWhiteSpace(provider, nameof(provider));
    }
    
    /// <summary>
    /// Marks the payment as failed
    /// </summary>
    public void MarkAsFailed(string reason)
    {
        Status = PaymentStatus.Failed;
        ProcessedDate = DateTime.UtcNow;
        Notes = reason;
    }
    
    /// <summary>
    /// Processes a refund for this payment
    /// </summary>
    public void ProcessRefund(decimal refundAmount, string reason)
    {
        Guard.Against.OutOfRange(refundAmount, nameof(refundAmount), 0, Amount);
        
        if (Status != PaymentStatus.Completed)
        {
            throw new InvalidOperationException("Can only refund completed payments");
        }
        
        IsRefunded = true;
        RefundedAmount = refundAmount;
        RefundDate = DateTime.UtcNow;
        RefundReason = reason;
        
        if (refundAmount == Amount)
        {
            Status = PaymentStatus.Refunded;
        }
        else
        {
            Status = PaymentStatus.PartiallyRefunded;
        }
    }
}