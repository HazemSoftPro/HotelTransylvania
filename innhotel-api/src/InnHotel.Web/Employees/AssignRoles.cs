using InnHotel.UseCases.Employees.AssignRoles;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Employees;

/// <summary>
/// Assign roles to an Employee.
/// </summary>
/// <remarks>
/// Assigns or updates the roles for a specific employee. Only SuperAdmin can assign roles.
/// </remarks>
public class AssignRoles(IMediator _mediator)
    : Endpoint<AssignEmployeeRolesRequest, object>
{
    public override void Configure()
    {
        Put(AssignEmployeeRolesRequest.Route);
        Roles(AuthRoles.SuperAdmin); // Only SuperAdmin can assign roles
        Summary(s =>
        {
            s.ExampleRequest = new AssignEmployeeRolesRequest
            {
                Id = 1,
                Roles = new List<string> { AuthRoles.Admin, AuthRoles.Receptionist }
            };
        });
    }

    public override async Task HandleAsync(
        AssignEmployeeRolesRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AssignEmployeeRolesCommand(request.Id, request.Roles);
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
                new { 
                    status = 200, 
                    message = "Employee roles assigned successfully" 
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
