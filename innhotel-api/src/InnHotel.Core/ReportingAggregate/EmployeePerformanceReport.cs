namespace InnHotel.Core.ReportingAggregate;

/// <summary>
/// Represents employee performance metrics
/// </summary>
public class EmployeePerformanceReport
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<EmployeeMetrics> EmployeeMetrics { get; set; } = new();
}

public class EmployeeMetrics
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int ReservationsProcessed { get; set; }
    public int CheckInsProcessed { get; set; }
    public int CheckOutsProcessed { get; set; }
    public int PaymentsProcessed { get; set; }
    public decimal TotalRevenueProcessed { get; set; }
    public double AverageProcessingTime { get; set; }
    public int GuestComplaints { get; set; }
    public double GuestSatisfactionScore { get; set; }
}