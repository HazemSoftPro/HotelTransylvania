namespace InnHotel.UseCases.Dashboard;

public record GetDashboardMetricsQuery(
    int? BranchId = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null) : IQuery<Result<DashboardMetricsDTO>>;