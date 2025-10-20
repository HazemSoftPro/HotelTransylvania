using InnHotel.Core.ReportingAggregate;
using InnHotel.Core.ReservationAggregate;
using InnHotel.Core.RoomAggregate;
using InnHotel.Core.PaymentAggregate;
using Microsoft.Extensions.Logging;

namespace InnHotel.Infrastructure.Reporting;

/// <summary>
/// Service for generating reports and analytics
/// </summary>
public class ReportingService : IReportingService
{
    private readonly IRepository<Reservation> _reservationRepository;
    private readonly IRepository<Room> _roomRepository;
    private readonly IRepository<Core.PaymentAggregate.Payment> _paymentRepository;
    private readonly ILogger<ReportingService> _logger;

    public ReportingService(
        IRepository<Reservation> reservationRepository,
        IRepository<Room> roomRepository,
        IRepository<Core.PaymentAggregate.Payment> paymentRepository,
        ILogger<ReportingService> logger)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
        _paymentRepository = paymentRepository;
        _logger = logger;
    }

    public async Task<OccupancyReport> GenerateOccupancyReportAsync(
        DateTime startDate,
        DateTime endDate,
        int? branchId = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Generating occupancy report from {StartDate} to {EndDate}",
            startDate, endDate);

        var rooms = await _roomRepository.ListAsync(cancellationToken);
        if (branchId.HasValue)
        {
            rooms = rooms.Where(r => r.BranchId == branchId.Value).ToList();
        }

        var reservations = await _reservationRepository.ListAsync(cancellationToken);
        reservations = reservations.Where(r =>
            r.CheckInDate.ToDateTime(TimeOnly.MinValue) <= endDate &&
            r.CheckOutDate.ToDateTime(TimeOnly.MinValue) >= startDate &&
            r.Status == ReservationStatus.CheckedIn
        ).ToList();

        var totalRooms = rooms.Count;
        var occupiedRooms = reservations.Count;
        var availableRooms = totalRooms - occupiedRooms;
        var occupancyRate = totalRooms > 0 ? (decimal)occupiedRooms / totalRooms * 100 : 0;

        var report = new OccupancyReport
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalRooms = totalRooms,
            OccupiedRooms = occupiedRooms,
            AvailableRooms = availableRooms,
            OccupancyRate = occupancyRate,
            TotalReservations = reservations.Count,
            CheckIns = reservations.Count(r => r.Status == ReservationStatus.CheckedIn),
            CheckOuts = reservations.Count(r => r.Status == ReservationStatus.CheckedOut)
        };

        // Generate daily breakdown
        var currentDate = startDate;
        while (currentDate <= endDate)
        {
            var dailyOccupied = reservations.Count(r =>
                r.CheckInDate.ToDateTime(TimeOnly.MinValue) <= currentDate &&
                r.CheckOutDate.ToDateTime(TimeOnly.MinValue) >= currentDate);

            var dailyOccupancyRate = totalRooms > 0 ? (decimal)dailyOccupied / totalRooms * 100 : 0;

            report.DailyBreakdown.Add(new DailyOccupancy
            {
                Date = currentDate,
                OccupiedRooms = dailyOccupied,
                AvailableRooms = totalRooms - dailyOccupied,
                OccupancyRate = dailyOccupancyRate,
                Revenue = 0 // Calculate from payments if needed
            });

            currentDate = currentDate.AddDays(1);
        }

        return report;
    }

    public async Task<RevenueReport> GenerateRevenueReportAsync(
        DateTime startDate,
        DateTime endDate,
        int? branchId = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Generating revenue report from {StartDate} to {EndDate}",
            startDate, endDate);

        var payments = await _paymentRepository.ListAsync(cancellationToken);
        payments = payments.Where(p =>
            p.PaymentDate >= startDate &&
            p.PaymentDate <= endDate &&
            p.Status == PaymentStatus.Completed
        ).ToList();

        var totalRevenue = payments.Sum(p => p.Amount);
        var totalRefunds = payments.Where(p => p.IsRefunded).Sum(p => p.RefundedAmount);
        var netRevenue = totalRevenue - totalRefunds;

        var report = new RevenueReport
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalRevenue = totalRevenue,
            TotalPayments = payments.Count,
            TotalRefunds = totalRefunds,
            NetRevenue = netRevenue,
            AverageRevenuePerReservation = payments.Count > 0 ? totalRevenue / payments.Count : 0
        };

        // Generate daily breakdown
        var currentDate = startDate;
        while (currentDate <= endDate)
        {
            var dailyPayments = payments.Where(p => p.PaymentDate.Date == currentDate.Date).ToList();
            var dailyRevenue = dailyPayments.Sum(p => p.Amount);

            report.DailyBreakdown.Add(new DailyRevenue
            {
                Date = currentDate,
                Revenue = dailyRevenue,
                Reservations = dailyPayments.Count,
                AverageRevenuePerReservation = dailyPayments.Count > 0 ? dailyRevenue / dailyPayments.Count : 0
            });

            currentDate = currentDate.AddDays(1);
        }

        // Payment method breakdown
        var paymentsByMethod = payments.GroupBy(p => p.Method);
        foreach (var group in paymentsByMethod)
        {
            var amount = group.Sum(p => p.Amount);
            report.PaymentMethodBreakdown.Add(new RevenueByPaymentMethod
            {
                PaymentMethod = group.Key.ToString(),
                Amount = amount,
                TransactionCount = group.Count(),
                Percentage = totalRevenue > 0 ? (amount / totalRevenue) * 100 : 0
            });
        }

        return report;
    }

    public async Task<EmployeePerformanceReport> GenerateEmployeePerformanceReportAsync(
        DateTime startDate,
        DateTime endDate,
        int? branchId = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Generating employee performance report from {StartDate} to {EndDate}",
            startDate, endDate);

        // This is a simplified implementation
        // In production, you would query actual employee activity data
        var report = new EmployeePerformanceReport
        {
            StartDate = startDate,
            EndDate = endDate,
            EmployeeMetrics = new List<EmployeeMetrics>()
        };

        return await Task.FromResult(report);
    }

    public async Task<byte[]> ExportToPdfAsync(
        object report,
        string reportTitle,
        CancellationToken cancellationToken = default)
    {
        // Mock implementation - integrate with PDF library in production
        _logger.LogInformation("Exporting report to PDF: {Title}", reportTitle);
        
        await Task.Delay(100, cancellationToken);
        
        // Return empty byte array as placeholder
        return Array.Empty<byte>();
    }

    public async Task<byte[]> ExportToExcelAsync(
        object report,
        string reportTitle,
        CancellationToken cancellationToken = default)
    {
        // Mock implementation - integrate with Excel library in production
        _logger.LogInformation("Exporting report to Excel: {Title}", reportTitle);
        
        await Task.Delay(100, cancellationToken);
        
        // Return empty byte array as placeholder
        return Array.Empty<byte>();
    }
}