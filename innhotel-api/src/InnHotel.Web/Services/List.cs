using InnHotel.UseCases.Services.List;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Services;

/// <summary>
/// List all Services.
/// </summary>
/// <remarks>
/// Retrieves a paginated list of all hotel services.
/// </remarks>
public class List(IMediator _mediator)
    : Endpoint<ListServiceRequest, object>
{
    public override void Configure()
    {
        Get(ListServiceRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin, AuthRoles.Employee);
    }

    public override async Task HandleAsync(
        ListServiceRequest request,
        CancellationToken cancellationToken)
    {
        var query = new ListServicesQuery(request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query, cancellationToken);

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
            var serviceRecords = result.Value.Select(s => new ServiceRecord(
                s.Id,
                s.BranchId,
                s.BranchName,
                s.Name,
                s.Price,
                s.Description)).ToList();

            await SendAsync(
                new { status = 200, message = "Services retrieved successfully", data = serviceRecords },
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
