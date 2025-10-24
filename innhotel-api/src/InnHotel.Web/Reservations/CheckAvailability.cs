using InnHotel.UseCases.Reservations.CheckAvailability;

namespace InnHotel.Web.Reservations;

public class CheckAvailability(IMediator mediator) : Endpoint<CheckAvailabilityRequest, CheckAvailabilityResponse>
{
    public override void Configure()
    {
        Get("/reservations/check-availability");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Check room availability";
            s.Description = "Checks if a room is available for the specified date range";
            s.ExampleRequest = new CheckAvailabilityRequest 
            { 
                RoomId = 1, 
                CheckInDate = "2025-01-01", 
                CheckOutDate = "2025-01-05" 
            };
        });
    }

    public override async Task<CheckAvailabilityResponse> ExecuteAsync(CheckAvailabilityRequest req, CancellationToken ct)
    {
        DateOnly checkInDate = default;
        DateOnly checkOutDate = default;

        if (!DateOnly.TryParse(req.CheckInDate, out checkInDate) ||
            !DateOnly.TryParse(req.CheckOutDate, out checkOutDate))
        {
            ThrowError("Invalid date format. Use YYYY-MM-DD format.");
        }

        var query = new CheckRoomAvailabilityQuery(
            req.RoomId,
            checkInDate,
            checkOutDate,
            req.ExcludeReservationId);

        var result = await mediator.Send(query, ct);

        return new CheckAvailabilityResponse
        {
            RoomId = req.RoomId,
            CheckInDate = req.CheckInDate,
            CheckOutDate = req.CheckOutDate,
            IsAvailable = result.Value
        };
    }
}

public class CheckAvailabilityRequest
{
    public int RoomId { get; set; }
    public string CheckInDate { get; set; } = string.Empty;
    public string CheckOutDate { get; set; } = string.Empty;
    public int? ExcludeReservationId { get; set; }
}

public class CheckAvailabilityResponse
{
    public int RoomId { get; set; }
    public string CheckInDate { get; set; } = string.Empty;
    public string CheckOutDate { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}