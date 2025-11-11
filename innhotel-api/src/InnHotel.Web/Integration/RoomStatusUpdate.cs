using Ardalis.ApiEndpoints;
using Ardalis.Result;
using InnHotel.Core.RoomAggregate;
using InnHotel.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnHotel.Web.Integration;

/// <summary>
/// Endpoint for updating room status with full integration
/// </summary>
[Authorize]
public class RoomStatusUpdate : EndpointBaseAsync
    .WithRequest<RoomStatusUpdateRequest>
    .WithActionResult<RoomStatusUpdateResponse>
{
    private readonly IIntegrationService _integrationService;
    private readonly ILogger<RoomStatusUpdate> _logger;

    public RoomStatusUpdate(IIntegrationService integrationService, ILogger<RoomStatusUpdate> logger)
    {
        _integrationService = integrationService;
        _logger = logger;
    }

    [HttpPost("api/integration/roomstatus")]
    public override async Task<ActionResult<RoomStatusUpdateResponse>> HandleAsync(
        RoomStatusUpdateRequest request, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating room {RoomId} status to {Status}", request.RoomId, request.NewStatus);

        try
        {
            await _integrationService.ProcessRoomStatusChangeAsync(
                request.RoomId, 
                request.NewStatus, 
                request.ChangedBy, 
                cancellationToken);

            var response = new RoomStatusUpdateResponse
            {
                Success = true,
                Message = "Room status updated successfully",
                RoomId = request.RoomId,
                OldStatus = request.OldStatus,
                NewStatus = request.NewStatus,
                UpdatedAt = DateTime.UtcNow
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating room {RoomId} status", request.RoomId);
            
            var response = new RoomStatusUpdateResponse
            {
                Success = false,
                Message = ex.Message,
                RoomId = request.RoomId,
                UpdatedAt = DateTime.UtcNow
            };

            return BadRequest(response);
        }
    }
}

public class RoomStatusUpdateRequest
{
    public int RoomId { get; set; }
    public RoomStatus NewStatus { get; set; }
    public RoomStatus? OldStatus { get; set; }
    public string? ChangedBy { get; set; }
    public string? Notes { get; set; }
}

public class RoomStatusUpdateResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public RoomStatus? OldStatus { get; set; }
    public RoomStatus NewStatus { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? RoomNumber { get; set; }
    public List<string> NotificationsSent { get; set; } = new();
}