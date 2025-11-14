using Ardalis.ApiEndpoints;
using Ardalis.Result;
using InnHotel.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnHotel.Web.Integration;

/// <summary>
/// Endpoint for processing guest check-out with full integration
/// </summary>
[Authorize]
public class CheckOut : EndpointBaseAsync
    .WithRequest<CheckOutRequest>
    .WithActionResult<CheckOutResponse>
{
    private readonly IIntegrationService _integrationService;
    private readonly ILogger<CheckOut> _logger;

    public CheckOut(IIntegrationService integrationService, ILogger<CheckOut> logger)
    {
        _integrationService = integrationService;
        _logger = logger;
    }

    [HttpPost("api/integration/checkout")]
    public override async Task<ActionResult<CheckOutResponse>> HandleAsync(
        CheckOutRequest request, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing check-out for reservation {ReservationId}", request.ReservationId);

        try
        {
            await _integrationService.ProcessGuestCheckOutAsync(request.ReservationId, cancellationToken);

            var response = new CheckOutResponse
            {
                Success = true,
                Message = "Check-out processed successfully",
                ReservationId = request.ReservationId,
                ProcessedAt = DateTime.UtcNow
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing check-out for reservation {ReservationId}", request.ReservationId);
            
            var response = new CheckOutResponse
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

public class CheckOutRequest
{
    public int ReservationId { get; set; }
    public string? Notes { get; set; }
    public decimal? FinalPaymentAmount { get; set; }
}

public class CheckOutResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ReservationId { get; set; }
    public DateTime ProcessedAt { get; set; }
    public decimal TotalBill { get; set; }
    public List<string> ReleasedRooms { get; set; } = new();
}