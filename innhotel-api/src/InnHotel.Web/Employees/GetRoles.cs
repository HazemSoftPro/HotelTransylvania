using InnHotel.UseCases.Employees.GetRoles;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Employees;

/// <summary>
/// Get Employee roles.
/// </summary>
/// <remarks>
/// Retrieves the roles assigned to a specific employee.
/// </remarks>
public class GetRoles(IMediator _mediator)
    : Endpoint<GetEmployeeRolesRequest, object>
{
    public override void Configure()
    {
        Get(GetEmployeeRolesRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin); // SuperAdmin and Admin can view roles
        Summary(s =>
        {
            s.ExampleRequest = new GetEmployeeRolesRequest
            {
                Id = 1
            };
        });
    }

    public override async Task HandleAsync(
        GetEmployeeRolesRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetEmployeeRolesQuery(request.Id);
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
            await SendAsync(
                new { 
                    status = 200, 
                    message = "Employee roles retrieved successfully",
                    data = new {
                        employeeId = request.Id,
                        roles = result.Value
                    }
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
