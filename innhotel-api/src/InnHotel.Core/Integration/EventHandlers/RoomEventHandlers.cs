using InnHotel.Core.Integration.Events;
using InnHotel.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace InnHotel.Core.Integration.EventHandlers;

/// <summary>
/// Handles room status change events
/// </summary>
public class RoomStatusChangedEventHandler : IIntegrationEventHandler<RoomStatusChangedEvent>
{
    private readonly ILogger<RoomStatusChangedEventHandler> _logger;
    private readonly INotificationService _notificationService;
    private readonly IReservationService _reservationService;
    private readonly IHousekeepingService _housekeepingService;

    public RoomStatusChangedEventHandler(
        ILogger<RoomStatusChangedEventHandler> logger,
        INotificationService notificationService,
        IReservationService reservationService,
        IHousekeepingService housekeepingService)
    {
        _logger = logger;
        _notificationService = notificationService;
        _reservationService = reservationService;
        _housekeepingService = housekeepingService;
    }

    public async Task HandleAsync(RoomStatusChangedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling RoomStatusChangedEvent for RoomId: {RoomId}, Status: {OldStatus} → {NewStatus}", 
            @event.RoomId, @event.OldStatus, @event.NewStatus);

        switch (@event.NewStatus)
        {
            case RoomStatus.Available:
                await HandleRoomBecameAvailable(@event, cancellationToken);
                break;
            case RoomStatus.Occupied:
                await HandleRoomBecameOccupied(@event, cancellationToken);
                break;
            case RoomStatus.Maintenance:
                await HandleRoomUnderMaintenance(@event, cancellationToken);
                break;
            case RoomStatus.Cleaning:
                await HandleRoomNeedsCleaning(@event, cancellationToken);
                break;
        }

        // Create general notification
        await _notificationService.CreateNotificationAsync(
            "Room Status Updated",
            $"Room {@event.RoomNumber} status changed from {@event.OldStatus} to {@event.NewStatus}",
            "RoomManagement",
            new Dictionary<string, object>
            {
                ["RoomId"] = @event.RoomId,
                ["RoomNumber"] = @event.RoomNumber,
                ["OldStatus"] = @event.OldStatus.ToString(),
                ["NewStatus"] = @event.NewStatus.ToString(),
                ["ChangedBy"] = @event.ChangedBy ?? "System"
            },
            cancellationToken);

        _logger.LogInformation("RoomStatusChangedEvent processed successfully");
    }

    private async Task HandleRoomBecameAvailable(RoomStatusChangedEvent @event, CancellationToken cancellationToken)
    {
        // Check if there are any waitlisted reservations for this room type
        var waitlistedReservations = await _reservationService.GetWaitlistedReservationsForRoomTypeAsync(
            @event.RoomId, cancellationToken);

        if (waitlistedReservations.Any())
        {
            await _notificationService.CreateNotificationAsync(
                "Room Available for Waitlist",
                $"Room {@event.RoomNumber} is now available for waitlisted guests",
                "FrontDesk",
                new Dictionary<string, object>
                {
                    ["RoomId"] = @event.RoomId,
                    ["RoomNumber"] = @event.RoomNumber,
                    ["WaitlistedCount"] = waitlistedReservations.Count()
                },
                cancellationToken);
        }
    }

    private async Task HandleRoomBecameOccupied(RoomStatusChangedEvent @event, CancellationToken cancellationToken)
    {
        // Update housekeeping schedules
        await _housekeepingService.UpdateRoomOccupancyAsync(@event.RoomId, true, cancellationToken);
    }

    private async Task HandleRoomUnderMaintenance(RoomStatusChangedEvent @event, CancellationToken cancellationToken)
    {
        // Check for upcoming reservations and notify front desk
        var upcomingReservations = await _reservationService.GetUpcomingReservationsForRoomAsync(
            @event.RoomId, cancellationToken);

        if (upcomingReservations.Any())
        {
            await _notificationService.CreateNotificationAsync(
                "Conflict: Room Under Maintenance",
                $"Room {@event.RoomNumber} has upcoming reservations but is under maintenance",
                "FrontDesk",
                new Dictionary<string, object>
                {
                    ["RoomId"] = @event.RoomId,
                    ["RoomNumber"] = @event.RoomNumber,
                    ["UpcomingReservations"] = upcomingReservations.Count()
                },
                cancellationToken, true); // High priority
        }
    }

    private async Task HandleRoomNeedsCleaning(RoomStatusChangedEvent @event, CancellationToken cancellationToken)
    {
        // Schedule housekeeping
        await _housekeepingService.ScheduleImmediateCleaningAsync(@event.RoomId, cancellationToken);
    }
}

/// <summary>
/// Handles housekeeping required events
/// </summary>
public class HousekeepingRequiredEventHandler : IIntegrationEventHandler<HousekeepingRequiredEvent>
{
    private readonly ILogger<HousekeepingRequiredEventHandler> _logger;
    private readonly INotificationService _notificationService;
    private readonly IHousekeepingService _housekeepingService;

    public HousekeepingRequiredEventHandler(
        ILogger<HousekeepingRequiredEventHandler> logger,
        INotificationService notificationService,
        IHousekeepingService housekeepingService)
    {
        _logger = logger;
        _notificationService = notificationService;
        _housekeepingService = housekeepingService;
    }

    public async Task HandleAsync(HousekeepingRequiredEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling HousekeepingRequiredEvent for RoomId: {RoomId}, Priority: {Priority}", 
            @event.RoomId, @event.Priority);

        // Assign housekeeping staff
        var assignment = await _housekeepingService.AssignHousekeepingStaffAsync(
            @event.RoomId, @event.Priority, @event.RequiredBy, cancellationToken);

        // Create notification for assigned staff
        if (assignment.StaffId.HasValue)
        {
            await _notificationService.CreateNotificationAsync(
                "New Housekeeping Assignment",
                $"Room {@event.RoomNumber} requires cleaning by {@event.RequiredBy:yyyy-MM-dd HH:mm}",
                "Housekeeping",
                new Dictionary<string, object>
                {
                    ["RoomId"] = @event.RoomId,
                    ["RoomNumber"] = @event.RoomNumber,
                    ["Priority"] = @event.Priority.ToString(),
                    ["RequiredBy"] = @event.RequiredBy,
                    ["StaffId"] = assignment.StaffId.Value,
                    ["SpecialInstructions"] = @event.SpecialInstructions ?? ""
                },
                cancellationToken);
        }
        else
        {
            // No staff available - notify supervisor
            await _notificationService.CreateNotificationAsync(
                "No Housekeeping Staff Available",
                $"Room {@event.RoomNumber} requires {@event.Priority} priority cleaning but no staff available",
                "HousekeepingSupervisor",
                new Dictionary<string, object>
                {
                    ["RoomId"] = @event.RoomId,
                    ["RoomNumber"] = @event.RoomNumber,
                    ["Priority"] = @event.Priority.ToString(),
                    ["RequiredBy"] = @event.RequiredBy
                },
                cancellationToken, true); // High priority
        }

        _logger.LogInformation("HousekeepingRequiredEvent processed successfully");
    }
}