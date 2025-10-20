namespace InnHotel.Core.ReportingAggregate;

/// <summary>
/// Represents revenue analytics for a given period
/// </summary>
public class RevenueReport
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal RoomRevenue { get; set; }
    public decimal ServiceRevenue { get; set; }
    public decimal AverageRevenuePerReservation { get; set; }
    public int TotalReservations { get; set; }
    public int TotalPayments { get; set; }
    public decimal TotalRefunds { get; set; }
    public decimal NetRevenue { get; set; }
    
    public List<DailyRevenue> DailyBreakdown { get; set; } = new();
    public List<RevenueByPaymentMethod> PaymentMethodBreakdown { get; set; } = new();
    public List<RevenueByRoomType> RoomTypeBreakdown { get; set; } = new();
}

public class DailyRevenue
{
    public DateTime Date { get; set; }
    public decimal Revenue { get; set; }
    public int Reservations { get; set; }
    public decimal AverageRevenuePerReservation { get; set; }
}

public class RevenueByPaymentMethod
{
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int TransactionCount { get; set; }
    public decimal Percentage { get; set; }
}

public class RevenueByRoomType
{
    public string RoomType { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int Reservations { get; set; }
    public decimal AverageRate { get; set; }
}