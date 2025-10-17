using InnHotel.UseCases.Dashboard;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Dashboard;

/// <summary>
/// Get Dashboard Metrics
/// </summary>
/// <remarks>
/// Retrieves comprehensive dashboard metrics including room statistics, reservation data, revenue, and recent activities.
/// </remarks>
public class GetMetrics(IMediator _mediator)
    : Endpoint<GetDashboardMetricsRequest, object>
{
    public override void Configure()
    {
        Get(GetDashboardMetricsRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin, AuthRoles.Employee);
        Summary(s =>
        {
            s.ExampleRequest = new GetDashboardMetricsRequest
            {
                BranchId = 1,
                StartDate = DateTime.UtcNow.AddMonths(-1),
                EndDate = DateTime.UtcNow
            };
        });
    }

    public override async Task HandleAsync(
        GetDashboardMetricsRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetDashboardMetricsQuery(
            request.BranchId,
            request.StartDate,
            request.EndDate);

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
            await SendAsync(
                new { 
                    status = 200, 
                    message = "Dashboard metrics retrieved successfully", 
                    data = result.Value
                },
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