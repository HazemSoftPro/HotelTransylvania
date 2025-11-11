using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InnHotel.Core.Services;
using InnHotel.Core.RoomAggregate;

namespace InnHotel.Web.Integration;

/// <summary>
/// Controller for validating business rules and cross-module constraints
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ValidationController : ControllerBase
{
    private readonly IIntegrationService _integrationService;
    private readonly ILogger<ValidationController> _logger;

    public ValidationController(
        IIntegrationService integrationService,
        ILogger<ValidationController> logger)
    {
        _integrationService = integrationService;
        _logger = logger;
    }

    /// <summary>
    /// Validates room availability for reservation
    /// </summary>
    [HttpPost("room-availability")]
    public async Task<ActionResult<ValidationResult>> ValidateRoomAvailability(
        [FromBody] RoomAvailabilityValidationRequest request)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["RoomIds"] = request.RoomIds,
                ["CheckInDate"] = request.CheckInDate,
                ["CheckOutDate"] = request.CheckOutDate
            };

            var result = await _integrationService.ValidateBusinessRulesAsync(
                "CreateReservation", 
                parameters);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating room availability");
            return BadRequest(ValidationResult.Failure("Validation service error"));
        }
    }

    /// <summary>
    /// Validates check-in requirements
    /// </summary>
    [HttpPost("checkin")]
    public async Task<ActionResult<ValidationResult>> ValidateCheckIn(
        [FromBody] CheckInValidationRequest request)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["ReservationId"] = request.ReservationId
            };

            var result = await _integrationService.ValidateBusinessRulesAsync(
                "CheckIn", 
                parameters);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating check-in");
            return BadRequest(ValidationResult.Failure("Validation service error"));
        }
    }

    /// <summary>
    /// Validates check-out requirements
    /// </summary>
    [HttpPost("checkout")]
    public async Task<ActionResult<ValidationResult>> ValidateCheckOut(
        [FromBody] CheckOutValidationRequest request)
    {
        try
        {
            var parameters = new Dictionary<string, object>
            {
                ["ReservationId"] = request.ReservationId
            };

            var result = await _integrationService.ValidateBusinessRulesAsync(
                "CheckOut", 
                parameters);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating check-out");
            return BadRequest(ValidationResult.Failure("Validation service error"));
        }
    }

    /// <summary>
    /// Validates room status change
    /// </summary>
    [HttpPost("room-status")]
    public async Task<ActionResult<ValidationResult>> ValidateRoomStatusChange(
        [FromBody] RoomStatusValidationRequest request)
    {
        try
        {
            var result = ValidationResult.Success();

            // Basic validation logic
            if (request.NewStatus == RoomStatus.Occupied && string.IsNullOrEmpty(request.GuestName))
            {
                result.Errors.Add("Guest name is required when marking room as occupied");
            }

            if (request.NewStatus == RoomStatus.Maintenance && request.MaintenanceEndDate.HasValue && request.MaintenanceEndDate <= DateTime.Today)
            {
                result.Errors.Add("Maintenance end date must be in the future");
            }

            if (request.NewStatus == RoomStatus.Available)
            {
                result.Warnings.Add("Ensure room is properly cleaned and inspected before marking as available");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating room status change");
            return BadRequest(ValidationResult.Failure("Validation service error"));
        }
    }
}

// Request models
public class RoomAvailabilityValidationRequest
{
    public int[] RoomIds { get; set; } = Array.Empty<int>();
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int GuestCount { get; set; }
}

public class CheckInValidationRequest
{
    public int ReservationId { get; set; }
    public int? GuestId { get; set; }
    public string? GuestName { get; set; }
}

public class CheckOutValidationRequest
{
    public int ReservationId { get; set; }
    public decimal? FinalPaymentAmount { get; set; }
    public List<string> OutstandingCharges { get; set; } = new();
}

public class RoomStatusValidationRequest
{
    public int RoomId { get; set; }
    public RoomStatus NewStatus { get; set; }
    public RoomStatus CurrentStatus { get; set; }
    public string? GuestName { get; set; }
    public DateTime? MaintenanceEndDate { get; set; }
    public string? ChangedBy { get; set; }
    public string? Notes { get; set; }
}