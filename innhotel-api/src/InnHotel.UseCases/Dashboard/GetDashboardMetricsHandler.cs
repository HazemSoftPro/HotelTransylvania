using InnHotel.Core.RoomAggregate;
using InnHotel.Core.ReservationAggregate;
using InnHotel.Core.GuestAggregate;

namespace InnHotel.UseCases.Dashboard;

public class GetDashboardMetricsHandler(
    IReadRepository<Room> _roomRepository,
    IReadRepository<Reservation> _reservationRepository,
    IReadRepository<Guest> _guestRepository)
    : IQueryHandler<GetDashboardMetricsQuery, Result<DashboardMetricsDTO>>
{
    public async Task<Result<DashboardMetricsDTO>> Handle(GetDashboardMetricsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var startDate = request.StartDate ?? DateTime.UtcNow.AddMonths(-1);
            var endDate = request.EndDate ?? DateTime.UtcNow;

            // Get room statistics
            var allRooms = await _roomRepository.ListAsync(cancellationToken);
            if (request.BranchId.HasValue)
            {
                allRooms = allRooms.Where(r => r.BranchId == request.BranchId.Value).ToList();
            }

            var totalRooms = allRooms.Count;
            var availableRooms = allRooms.Count(r => r.Status == RoomStatus.Available);
            var occupiedRooms = allRooms.Count(r => r.Status == RoomStatus.Occupied);
            var maintenanceRooms = allRooms.Count(r => r.Status == RoomStatus.UnderMaintenance);
            var occupancyRate = totalRooms > 0 ? (decimal)occupiedRooms / totalRooms * 100 : 0;

            // Get reservation statistics
            var allReservations = await _reservationRepository.ListAsync(cancellationToken);

            var totalReservations = allReservations.Count;
            var activeReservations = allReservations.Count(r => 
                r.Status == ReservationStatus.Confirmed || 
                r.Status == ReservationStatus.CheckedIn);
            var pendingReservations = allReservations.Count(r => r.Status == ReservationStatus.Pending);
            var confirmedReservations = allReservations.Count(r => r.Status == ReservationStatus.Confirmed);
            var checkedInReservations = allReservations.Count(r => r.Status == ReservationStatus.CheckedIn);

            // Calculate revenue
            var completedReservations = allReservations.Where(r => r.Status == ReservationStatus.CheckedOut);
            var totalRevenue = completedReservations.Sum(r => r.TotalCost);
            
            var monthlyReservations = completedReservations.Where(r => 
                r.CheckOutDate >= DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1)));
            var monthlyRevenue = monthlyReservations.Sum(r => r.TotalCost);

            // Get guest statistics
            var allGuests = await _guestRepository.ListAsync(cancellationToken);
            var totalGuests = allGuests.Count;
            var newGuestsThisMonth = 0; // TODO: Implement when CreatedDate is added to Guest

            // Get recent activities (last 10)
            var recentReservations = allReservations
                .OrderByDescending(r => r.ReservationDate)
                .Take(10)
                .Select(r => new RecentActivityDTO(
                    "Reservation",
                    $"New reservation for {r.Guest?.FirstName} {r.Guest?.LastName}",
                    r.ReservationDate,
                    r.Id.ToString()))
                .ToArray();

            var metrics = new DashboardMetricsDTO(
                totalRooms,
                availableRooms,
                occupiedRooms,
                maintenanceRooms,
                Math.Round(occupancyRate, 2),
                totalReservations,
                activeReservations,
                pendingReservations,
                confirmedReservations,
                checkedInReservations,
                totalRevenue,
                monthlyRevenue,
                totalGuests,
                newGuestsThisMonth,
                recentReservations);

            return Result.Success(metrics);
        }
        catch (Exception ex)
        {
            return Result.Error($"Failed to retrieve dashboard metrics: {ex.Message}");
        }
    }
}