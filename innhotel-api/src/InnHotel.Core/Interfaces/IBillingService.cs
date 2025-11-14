namespace InnHotel.Core.Interfaces;

/// <summary>
/// Service for managing billing and payments
/// </summary>
public interface IBillingService
{
    Task FinalizeBillAsync(int reservationId, decimal totalAmount, CancellationToken cancellationToken = default);
    Task GenerateInvoiceAsync(int reservationId, CancellationToken cancellationToken = default);
    Task<BillSummary> GetBillSummaryAsync(int reservationId, CancellationToken cancellationToken = default);
    Task ProcessPaymentAsync(int reservationId, decimal amount, string paymentMethod, CancellationToken cancellationToken = default);
}

/// <summary>
/// Summary of a bill
/// </summary>
public class BillSummary
{
    public int ReservationId { get; set; }
    public decimal RoomCharges { get; set; }
    public decimal ServiceCharges { get; set; }
    public decimal Taxes { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal OutstandingAmount { get; set; }
    public List<BillItem> Items { get; set; } = new();
}

/// <summary>
/// Individual bill item
/// </summary>
public class BillItem
{
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime DateAdded { get; set; }
}