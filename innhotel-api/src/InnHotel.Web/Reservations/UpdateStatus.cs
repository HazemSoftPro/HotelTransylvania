using InnHotel.UseCases.Reservations.UpdateStatus;

namespace InnHotel.Web.Reservations;

public class UpdateStatus(IMediator mediator) : Endpoint<UpdateReservationStatusRequest, Result>
{
    public override void Configure()
    {
        Put("/reservations/{id}/status");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Update reservation status";
            s.Description = "Updates the status of a reservation with validation for proper status transitions";
            s.ExampleRequest = new UpdateReservationStatusRequest { NewStatus = "CheckedIn" };
        });
    }

    public override async Task<Result> ExecuteAsync(UpdateReservationStatusRequest req, CancellationToken ct)
    {
        var command = new UpdateReservationStatusCommand(req.Id, req.NewStatus);
        var result = await mediator.Send(command, ct);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return result;
        }

        if (result.Status == ResultStatus.Invalid)
        {
            await SendAsync(result, statusCode: 400, cancellation: ct);
            return result;
        }

        return result;
    }
}

public class UpdateReservationStatusRequest
{
    public int Id { get; set; }
    public string NewStatus { get; set; } = string.Empty;
}