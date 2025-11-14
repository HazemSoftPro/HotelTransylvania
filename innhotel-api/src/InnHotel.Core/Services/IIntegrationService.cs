using InnHotel.Core.Integration.Events;

namespace InnHotel.Core.Services;

/// <summary>
/// Interface for integration services that handle cross-module business logic
/// </summary>
public interface IIntegrationService
{
    /// <summary>
    /// Processes guest check-in with all necessary cross-module updates
    /// </summary>
    Task ProcessGuestCheckInAsync(int reservationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes guest check-out with all necessary cross-module updates
    /// </summary>
    Task ProcessGuestCheckOutAsync(int reservationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes room status change with all necessary cross-module updates
    /// </summary>
    Task ProcessRoomStatusChangeAsync(int roomId, RoomStatus newStatus, string? changedBy = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes reservation creation with room availability checks and blocking
    /// </summary>
    Task ProcessReservationCreationAsync(int reservationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes reservation cancellation with room release and refund processing
    /// </summary>
    Task ProcessReservationCancellationAsync(int reservationId, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes payment completion with billing and notification updates
    /// </summary>
    Task ProcessPaymentCompletionAsync(int paymentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates cross-module business rules
    /// </summary>
    Task<ValidationResult> ValidateBusinessRulesAsync(string operationType, Dictionary<string, object> parameters, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of business rule validation
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public Dictionary<string, object> AdditionalData { get; set; } = new();

    public static ValidationResult Success() => new() { IsValid = true };
    
    public static ValidationResult Failure(params string[] errors) => new() 
    { 
        IsValid = false, 
        Errors = errors.ToList() 
    }
}