using InnHotel.UseCases.Services.Delete;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Services;

/// <summary>
/// Delete a Service.
/// </summary>
/// <remarks>
/// Deletes an existing hotel service by its ID.
/// </remarks>
public class Delete(IMediator _mediator)
    : Endpoint<DeleteServiceRequest, object>
{
    public override void Configure()
    {
        Delete(DeleteServiceRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin);
    }

    public override async Task HandleAsync(
        DeleteServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new DeleteServiceCommand(request.Id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendAsync(
                new FailureResponse(404, result.Errors.First()),
                statusCode: 404,
                cancellation: cancellationToken);
            return;
        }

        if (result.Status == ResultStatus.Error)
        {
            await SendAsync(
                new FailureResponse(400, result.Errors.First()),
                statusCode: 400,
                cancellation: cancellationToken);
            return;
        }

        if (result.IsSuccess)
        {
            await SendAsync(
                new { status = 200, message = "Service deleted successfully" },
                statusCode: 200,
                cancellation: cancellationToken);
            return;
        }

        await SendAsync(
            new FailureResponse(500, "An unexpected error occurred."),
            statusCode: 500,
            cancellation: cancellationToken);
    }
}
