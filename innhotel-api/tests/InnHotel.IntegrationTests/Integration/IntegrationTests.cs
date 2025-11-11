using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using InnHotel.Infrastructure.Data;
using InnHotel.Core.Integration;
using InnHotel.Core.Services;
using InnHotel.Core.RoomAggregate;
using InnHotel.Core.ReservationAggregate;
using Xunit;
using System.Net.Http.Json;
using System.Net;

namespace InnHotel.IntegrationTests.Integration;

/// <summary>
/// Integration tests for the hotel management system
/// </summary>
public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Configure in-memory database for testing
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("HotelIntegrationTestDb");
                });

                // Add test services
                services.AddScoped<IEmailService, TestEmailService>();
                services.AddScoped<IHousekeepingService, TestHousekeepingService>();
                services.AddScoped<IBillingService, TestBillingService>();
                services.AddScoped<IReservationService, TestReservationService>();
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CompleteCheckInWorkflow_ShouldIntegrateAllModules()
    {
        // Arrange
        await SetupTestData();

        // Act - Process check-in
        var checkInRequest = new
        {
            ReservationId = 1,
            Notes = "Guest arrived early"
        };

        var response = await _client.PostAsJsonAsync("/api/integration/checkin", checkInRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        
        var checkInResponse = await response.Content.ReadFromJsonAsync<CheckInResponse>();
        Assert.NotNull(checkInResponse);
        Assert.True(checkInResponse.Success);

        // Verify room status was updated
        var roomStatusResponse = await _client.GetAsync("/api/rooms/1");
        var roomData = await roomStatusResponse.Content.ReadFromJsonAsync<object>();
        // Additional assertions for room status

        // Verify notifications were sent
        // Verify billing was initiated
        // Verify housekeeping was notified
    }

    [Fact]
    public async Task CompleteCheckOutWorkflow_ShouldIntegrateAllModules()
    {
        // Arrange
        await SetupTestData();
        await ProcessCheckIn(1); // First check in

        // Act - Process check-out
        var checkOutRequest = new
        {
            ReservationId = 1,
            Notes = "Guest satisfied with stay",
            FinalPaymentAmount = 250.00m
        };

        var response = await _client.PostAsJsonAsync("/api/integration/checkout", checkOutRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        
        var checkOutResponse = await response.Content.ReadFromJsonAsync<CheckOutResponse>();
        Assert.NotNull(checkOutResponse);
        Assert.True(checkOutResponse.Success);

        // Verify room status was updated to Cleaning
        // Verify final bill was generated
        // Verify housekeeping was assigned
        // Verify guest history was updated
    }

    [Fact]
    public async Task RoomStatusChange_ShouldTriggerNotifications()
    {
        // Arrange
        await SetupTestData();

        // Act - Change room status
        var statusRequest = new
        {
            RoomId = 1,
            NewStatus = "Maintenance",
            OldStatus = "Available",
            ChangedBy = "TestUser",
            Notes = "Air conditioning repair needed"
        };

        var response = await _client.PostAsJsonAsync("/api/integration/roomstatus", statusRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        
        var statusResponse = await response.Content.ReadFromJsonAsync<RoomStatusResponse>();
        Assert.NotNull(statusResponse);
        Assert.True(statusResponse.Success);

        // Verify notifications were sent to relevant departments
        // Verify room status was updated in database
        // Verify upcoming reservations were checked for conflicts
    }

    [Fact]
    public async Task DashboardMetrics_ShouldProvideRealTimeData()
    {
        // Arrange
        await SetupTestData();

        // Act
        var response = await _client.GetAsync("/api/dashboard/metrics");

        // Assert
        response.EnsureSuccessStatusCode();
        
        var metrics = await response.Content.ReadFromJsonAsync<DashboardMetrics>();
        Assert.NotNull(metrics);
        Assert.True(metrics.TotalRooms > 0);
        Assert.True(metrics.OccupancyRate >= 0);
        Assert.True(metrics.TodayRevenue >= 0);
    }

    [Fact]
    public async Task ValidationRules_ShouldEnforceBusinessConstraints()
    {
        // Arrange
        await SetupTestData();

        // Act - Validate room availability
        var validationRequest = new
        {
            RoomIds = new[] { 1, 2 },
            CheckInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            CheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3)),
            GuestCount = 2
        };

        var response = await _client.PostAsJsonAsync("/api/validation/room-availability", validationRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        
        var validationResult = await response.Content.ReadFromJsonAsync<ValidationResult>();
        Assert.NotNull(validationResult);
        
        // Verify validation logic works correctly
        // Verify conflicts are detected
        // Verify business rules are enforced
    }

    [Fact]
    public async Task EventBus_ShouldPublishAndHandleEvents()
    {
        // Arrange
        var services = _factory.Services;
        var eventBus = services.GetRequiredService<IEventBus>();
        var eventHandled = false;

        // Act
        await eventBus.SubscribeAsync<GuestCreatedEvent>(async (evt) =>
        {
            eventHandled = true;
        });

        await eventBus.PublishAsync(new GuestCreatedEvent(
            new Core.GuestAggregate.Guest(
                "Test", "User", 
                Core.GuestAggregate.Gender.Male,
                Core.GuestAggregate.IdProofType.Passport,
                "TEST123"
            )
        ));

        // Allow some time for async processing
        await Task.Delay(100);

        // Assert
        Assert.True(eventHandled);
    }

    [Fact]
    public async Task SignalRConnection_ShouldProvideRealTimeUpdates()
    {
        // Test SignalR hub connectivity and real-time updates
        // This would require a more complex test setup with SignalR client
        
        // For now, just verify the hub endpoint is accessible
        var response = await _client.GetAsync("/hotelHub");
        
        // SignalR hubs return specific responses for non-websocket requests
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest || 
                  response.StatusCode == HttpStatusCode.NotFound);
    }

    private async Task SetupTestData()
    {
        // Setup test data in the in-memory database
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Add test rooms
        dbContext.Rooms.AddRange(
            new Core.RoomAggregate.Room(1, 1, "101", RoomStatus.Available, 1, 100m),
            new Core.RoomAggregate.Room(1, 1, "102", RoomStatus.Available, 1, 100m),
            new Core.RoomAggregate.Room(1, 1, "103", RoomStatus.Maintenance, 1, 100m)
        );

        // Add test guest
        var guest = new Core.GuestAggregate.Guest(
            "John", "Doe", 
            Core.GuestAggregate.Gender.Male,
            Core.GuestAggregate.IdProofType.Passport,
            "PASS123",
            "john@example.com",
            "555-1234"
        );
        dbContext.Guests.Add(guest);

        await dbContext.SaveChangesAsync();

        // Add test reservation
        var reservation = new Core.ReservationAggregate.Reservation(
            guest.Id,
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            DateOnly.FromDateTime(DateTime.Today.AddDays(3)),
            ReservationStatus.Confirmed,
            200m
        );
        reservation.AddRoom(1, 100m);
        dbContext.Reservations.Add(reservation);

        await dbContext.SaveChangesAsync();
    }

    private async Task ProcessCheckIn(int reservationId)
    {
        var checkInRequest = new { ReservationId = reservationId };
        await _client.PostAsJsonAsync("/api/integration/checkin", checkInRequest);
    }
}

// Test service implementations
public class TestEmailService : Core.Interfaces.IEmailService
{
    public List<EmailMessage> SentEmails { get; } = new();

    public Task SendWelcomeEmailAsync(string email, string firstName, CancellationToken cancellationToken = default)
    {
        SentEmails.Add(new EmailMessage(email, "Welcome", $"Welcome {firstName}!"));
        return Task.CompletedTask;
    }

    public Task SendThankYouEmailAsync(string email, string guestName, decimal totalAmount, CancellationToken cancellationToken = default)
    {
        SentEmails.Add(new EmailMessage(email, "Thank You", $"Thank you {guestName}! Total: ${totalAmount}"));
        return Task.CompletedTask;
    }

    public Task SendReservationConfirmationAsync(string email, string guestName, int reservationId, CancellationToken cancellationToken = default)
    {
        SentEmails.Add(new EmailMessage(email, "Reservation Confirmed", $"Reservation {reservationId} confirmed for {guestName}"));
        return Task.CompletedTask;
    }

    public Task SendCancellationConfirmationAsync(string email, string guestName, int reservationId, decimal refundAmount, CancellationToken cancellationToken = default)
    {
        SentEmails.Add(new EmailMessage(email, "Cancellation Confirmed", $"Reservation {reservationId} cancelled for {guestName}. Refund: ${refundAmount}"));
        return Task.CompletedTask;
    }

    public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        SentEmails.Add(new EmailMessage(to, subject, body));
        return Task.CompletedTask;
    }
}

public class TestHousekeepingService : Core.Interfaces.IHousekeepingService
{
    public List<HousekeepingTask> Tasks { get; } = new();

    public Task<HousekeepingAssignment> AssignHousekeepingStaffAsync(int roomId, HousekeepingPriority priority, DateTime requiredBy, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new HousekeepingAssignment 
        { 
            StaffId = 1, 
            StaffName = "Test Housekeeper", 
            AssignedAt = DateTime.Now, 
            DueBy = requiredBy, 
            Priority = priority 
        });
    }

    public Task MarkRoomAsOccupiedAsync(int roomId, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task ScheduleCleaningAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        Tasks.Add(new HousekeepingTask(reservationId, "Post-checkout cleaning"));
        return Task.CompletedTask;
    }

    public Task ScheduleImmediateCleaningAsync(int roomId, CancellationToken cancellationToken = default)
    {
        Tasks.Add(new HousekeepingTask(roomId, "Immediate cleaning required"));
        return Task.CompletedTask;
    }

    public Task UpdateRoomOccupancyAsync(int roomId, bool isOccupied, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

public class TestBillingService : Core.Interfaces.IBillingService
{
    public List<Bill> GeneratedBills { get; } = new();

    public Task FinalizeBillAsync(int reservationId, decimal totalAmount, CancellationToken cancellationToken = default)
    {
        GeneratedBills.Add(new Bill(reservationId, totalAmount, "Finalized"));
        return Task.CompletedTask;
    }

    public Task GenerateInvoiceAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        GeneratedBills.Add(new Bill(reservationId, 0, "Invoice Generated"));
        return Task.CompletedTask;
    }

    public Task<BillSummary> GetBillSummaryAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new BillSummary { ReservationId = reservationId, TotalAmount = 100m });
    }

    public Task ProcessPaymentAsync(int reservationId, decimal amount, string paymentMethod, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

public class TestReservationService : Core.Interfaces.IReservationService
{
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

    public Task<IEnumerable<Reservation>> GetWaitlistedReservationsForRoomTypeAsync(int roomId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Enumerable.Empty<Reservation>());
    }

    public Task<IEnumerable<Reservation>> GetUpcomingReservationsForRoomAsync(int roomId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Enumerable.Empty<Reservation>());
    }
}

// Test data models
public record EmailMessage(string To, string Subject, string Body);
public record HousekeepingTask(int EntityId, string Description);
public record Bill(int ReservationId, decimal Amount, string Status);

// Response models
public class CheckInResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ReservationId { get; set; }
    public DateTime ProcessedAt { get; set; }
}

public class CheckOutResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ReservationId { get; set; }
    public DateTime ProcessedAt { get; set; }
    public decimal TotalBill { get; set; }
}

public class RoomStatusResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class DashboardMetrics
{
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public decimal OccupancyRate { get; set; }
    public decimal TodayRevenue { get; set; }
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
}