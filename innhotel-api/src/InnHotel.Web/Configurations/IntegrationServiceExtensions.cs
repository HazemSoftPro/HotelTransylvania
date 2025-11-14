using InnHotel.Core.Integration;
using InnHotel.Core.Integration.EventHandlers;
using InnHotel.Core.Services;
using InnHotel.Infrastructure.Integration;
using InnHotel.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace InnHotel.Web.Configurations;

/// <summary>
/// Extension methods for registering integration services
/// </summary>
public static class IntegrationServiceExtensions
{
    /// <summary>
    /// Adds integration services to the service collection
    /// </summary>
    public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        // Register Event Bus
        services.AddSingleton<IEventBus, InMemoryEventBus>();

        // Register Integration Service
        services.AddScoped<IIntegrationService, IntegrationService>();

        // Register Event Handlers
        services.AddScoped<IIntegrationEventHandler<GuestCreatedEvent>, GuestCreatedEventHandler>();
        services.AddScoped<IIntegrationEventHandler<GuestUpdatedEvent>, GuestUpdatedEventHandler>();
        services.AddScoped<IIntegrationEventHandler<GuestCheckedInEvent>, GuestCheckedInEventHandler>();
        services.AddScoped<IIntegrationEventHandler<GuestCheckedOutEvent>, GuestCheckedOutEventHandler>();

        services.AddScoped<IIntegrationEventHandler<RoomStatusChangedEvent>, RoomStatusChangedEventHandler>();
        services.AddScoped<IIntegrationEventHandler<RoomAssignedEvent>, RoomAssignedEventHandler>();
        services.AddScoped<IIntegrationEventHandler<RoomBecameAvailableEvent>, RoomBecameAvailableEventHandler>();
        services.AddScoped<IIntegrationEventHandler<HousekeepingRequiredEvent>, HousekeepingRequiredEventHandler>();

        services.AddScoped<IIntegrationEventHandler<ReservationCreatedEvent>, ReservationCreatedEventHandler>();
        services.AddScoped<IIntegrationEventHandler<ReservationModifiedEvent>, ReservationModifiedEventHandler>();
        services.AddScoped<IIntegrationEventHandler<ReservationCancelledEvent>, ReservationCancelledEventHandler>();
        services.AddScoped<IIntegrationEventHandler<ReservationConfirmedEvent>, ReservationConfirmedEventHandler>();

        services.AddScoped<IIntegrationEventHandler<PaymentProcessedEvent>, PaymentProcessedEventHandler>();
        services.AddScoped<IIntegrationEventHandler<PaymentFailedEvent>, PaymentFailedEventHandler>();
        services.AddScoped<IIntegrationEventHandler<RefundProcessedEvent>, RefundProcessedEventHandler>();

        // Register SignalR Hub and Notification Service
        services.AddSignalR();
        services.AddScoped<INotificationService, HotelNotificationService>();

        // Register additional services (these would need to be implemented)
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IHousekeepingService, HousekeepingService>();
        services.AddScoped<IBillingService, BillingService>();
        services.AddScoped<IReservationService, ReservationService>();

        return services;
    }

    /// <summary>
    /// Configures SignalR hubs
    /// </summary>
    public static IApplicationBuilder UseIntegrationHubs(this IApplicationBuilder app)
    {
        app.UseRouting();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<HotelHub>"/hotelHub");
        });

        return app;
    }
}

// Placeholder implementations - these would need to be fully implemented
namespace InnHotel.Web.Services
{
    public class EmailService : InnHotel.Core.Interfaces.IEmailService
    {
        public Task SendWelcomeEmailAsync(string email, string firstName, CancellationToken cancellationToken = default)
        {
            // Implementation would send actual email
            return Task.CompletedTask;
        }

        public Task SendThankYouEmailAsync(string email, string guestName, decimal totalAmount, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task SendReservationConfirmationAsync(string email, string guestName, int reservationId, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task SendCancellationConfirmationAsync(string email, string guestName, int reservationId, decimal refundAmount, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    public class HousekeepingService : InnHotel.Core.Interfaces.IHousekeepingService
    {
        public Task<HousekeepingAssignment> AssignHousekeepingStaffAsync(int roomId, HousekeepingPriority priority, DateTime requiredBy, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new HousekeepingAssignment { StaffId = 1, StaffName = "Housekeeper 1", AssignedAt = DateTime.Now, DueBy = requiredBy, Priority = priority });
        }

        public Task MarkRoomAsOccupiedAsync(int roomId, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task ScheduleCleaningAsync(int reservationId, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task ScheduleImmediateCleaningAsync(int roomId, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task UpdateRoomOccupancyAsync(int roomId, bool isOccupied, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    public class BillingService : InnHotel.Core.Interfaces.IBillingService
    {
        public Task FinalizeBillAsync(int reservationId, decimal totalAmount, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task GenerateInvoiceAsync(int reservationId, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<BillSummary> GetBillSummaryAsync(int reservationId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new BillSummary { ReservationId = reservationId, TotalAmount = 0 });
        }

        public Task ProcessPaymentAsync(int reservationId, decimal amount, string paymentMethod, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    public class ReservationService : InnHotel.Core.Interfaces.IReservationService
    {
        public Task<IEnumerable<Reservation>> GetWaitlistedReservationsForRoomTypeAsync(int roomId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Enumerable.Empty<Reservation>());
        }

        public Task<IEnumerable<Reservation>> GetUpcomingReservationsForRoomAsync(int roomId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Enumerable.Empty<Reservation>());
        }

        public Task<bool> IsRoomAvailableAsync(int roomId, DateOnly checkInDate, DateOnly checkOutDate, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        public Task<Reservation?> GetReservationByIdAsync(int reservationId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Reservation?>(null);
        }

        public Task<IEnumerable<Reservation>> GetReservationsByGuestIdAsync(int guestId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Enumerable.Empty<Reservation>());
        }
    }
}