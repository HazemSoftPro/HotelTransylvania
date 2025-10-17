namespace InnHotel.UseCases.Dashboard;

/// <summary>
/// Data Transfer Object for Dashboard Metrics
/// </summary>
public record DashboardMetricsDTO(
    int TotalRooms,
    int AvailableRooms,
    int OccupiedRooms,
    int MaintenanceRooms,
    decimal OccupancyRate,
    int TotalReservations,
    int ActiveReservations,
    int PendingReservations,
    int ConfirmedReservations,
    int CheckedInReservations,
    decimal TotalRevenue,
    decimal MonthlyRevenue,
    int TotalGuests,
    int NewGuestsThisMonth,
    RecentActivityDTO[] RecentActivities);

public record RecentActivityDTO(
    string Type,
    string Description,
    DateTime Timestamp,
    string? EntityId);