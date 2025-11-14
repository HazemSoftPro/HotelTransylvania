using Ardalis.ApiEndpoints;
using Ardalis.Result;
using InnHotel.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnHotel.Web.Integration;

/// <summary>
/// Endpoint for processing guest check-in with full integration
/// </summary>
[Authorize]
public class CheckIn : EndpointBaseAsync
    .WithRequest<CheckInRequest>
    .WithActionResult<CheckInResponse>
{
    private readonly IIntegrationService _integrationService;
    private readonly ILogger<CheckIn> _logger;

    public CheckIn(IIntegrationService integrationService, ILogger<CheckIn> logger)
    {
        _integrationService = integrationService;
        _logger = logger;
    }

    [HttpPost("api/integration/checkin")]
    public override async Task<ActionResult<CheckInResponse>> HandleAsync(
        CheckInRequest request, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing check-in for reservation {ReservationId}", request.ReservationId);

        try
        {
            await _integrationService.ProcessGuestCheckInAsync(request.ReservationId, cancellationToken);

            var response = new CheckInResponse
            {
                Success = true,
                Message = "Check-in processed successfully",
                ReservationId = request.ReservationId,
                ProcessedAt = DateTime.UtcNow
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing check-in for reservation {ReservationId}", request.ReservationId);
            
            var response = new CheckInResponse
            {
                Success = false,
                Message = ex.Message,
                ReservationId = request.ReservationId,
                ProcessedAt = DateTime.UtcNow
            };

            return BadRequest(response);
        }
    }
}

public class CheckInRequest
{
    public int ReservationId { get; set; }
    public string? Notes { get; set; }
}

public class CheckInResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ReservationId { get; set; }
    public DateTime ProcessedAt { get; set; }
    public List<string> RoomNumbers { get; set; } = new();
}