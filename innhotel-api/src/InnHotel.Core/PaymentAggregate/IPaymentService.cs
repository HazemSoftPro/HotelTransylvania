namespace InnHotel.Core.PaymentAggregate;

/// <summary>
/// Interface for payment processing services
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Processes a payment transaction
    /// </summary>
    Task<PaymentResult> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Processes a refund for a payment
    /// </summary>
    Task<PaymentResult> ProcessRefundAsync(Payment payment, decimal refundAmount, string reason, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Validates payment details before processing
    /// </summary>
    Task<bool> ValidatePaymentAsync(Payment payment, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of a payment operation
/// </summary>
public class PaymentResult
{
    public bool Success { get; set; }
    public string? TransactionId { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Provider { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
}