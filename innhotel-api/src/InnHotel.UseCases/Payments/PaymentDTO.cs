namespace InnHotel.UseCases.Payments;

public class PaymentDTO
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string? TransactionId { get; set; }
    public string? PaymentProvider { get; set; }
    public bool IsRefunded { get; set; }
    public decimal RefundedAmount { get; set; }
    public DateTime? RefundDate { get; set; }
    public string? RefundReason { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? GuestName { get; set; }
}