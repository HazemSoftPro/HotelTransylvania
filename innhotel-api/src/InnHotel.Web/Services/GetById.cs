using InnHotel.UseCases.Services.Get;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Services;

/// <summary>
/// Get a Service by ID.
/// </summary>
/// <remarks>
/// Retrieves details of a specific hotel service by its ID.
/// </remarks>
public class GetById(IMediator _mediator)
    : Endpoint<GetServiceByIdRequest, object>
{
    public override void Configure()
    {
        Get(GetServiceByIdRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin, AuthRoles.Employee);
    }

    public override async Task HandleAsync(
        GetServiceByIdRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetServiceByIdQuery(request.Id);
        var result = await _mediator.Send(query, cancellationToken);

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
            var serviceRecord = new ServiceRecord(
                result.Value.Id,
                result.Value.BranchId,
                result.Value.BranchName,
                result.Value.Name,
                result.Value.Price,
                result.Value.Description);

            await SendAsync(
                new { status = 200, message = "Service retrieved successfully", data = serviceRecord },
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
