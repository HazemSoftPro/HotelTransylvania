namespace InnHotel.Web.Dashboard;

public class GetDashboardMetricsRequest
{
    public const string Route = "/api/dashboard/metrics";
    
    public int? BranchId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}