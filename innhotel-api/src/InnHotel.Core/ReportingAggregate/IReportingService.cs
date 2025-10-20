namespace InnHotel.Core.ReportingAggregate;

/// <summary>
/// Service for generating various reports and analytics
/// </summary>
public interface IReportingService
{
    /// <summary>
    /// Generates occupancy report for the specified period
    /// </summary>
    Task<OccupancyReport> GenerateOccupancyReportAsync(
        DateTime startDate, 
        DateTime endDate, 
        int? branchId = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Generates revenue report for the specified period
    /// </summary>
    Task<RevenueReport> GenerateRevenueReportAsync(
        DateTime startDate, 
        DateTime endDate, 
        int? branchId = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Generates employee performance report
    /// </summary>
    Task<EmployeePerformanceReport> GenerateEmployeePerformanceReportAsync(
        DateTime startDate, 
        DateTime endDate, 
        int? branchId = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Exports report to PDF format
    /// </summary>
    Task<byte[]> ExportToPdfAsync(
        object report, 
        string reportTitle,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Exports report to Excel format
    /// </summary>
    Task<byte[]> ExportToExcelAsync(
        object report, 
        string reportTitle,
        CancellationToken cancellationToken = default);
}