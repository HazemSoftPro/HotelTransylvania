using InnHotel.UseCases.Employees.Search;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Employees;

/// <summary>
/// Search Employees with filters.
/// </summary>
/// <remarks>
/// Searches and filters employees based on various criteria including search term, branch, position, and hire date range.
/// </remarks>
public class Search(IMediator _mediator)
    : Endpoint<SearchEmployeesRequest, object>
{
    public override void Configure()
    {
        Get(SearchEmployeesRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin);
        Summary(s =>
        {
            s.ExampleRequest = new SearchEmployeesRequest
            {
                SearchTerm = "John",
                BranchId = 1,
                Position = "Manager",
                HireDateFrom = DateTime.UtcNow.AddYears(-1),
                HireDateTo = DateTime.UtcNow,
                PageNumber = 1,
                PageSize = 10
            };
        });
    }

    public override async Task HandleAsync(
        SearchEmployeesRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchEmployeesQuery(
            request.SearchTerm,
            request.BranchId,
            request.Position,
            request.HireDateFrom,
            request.HireDateTo,
            request.PageNumber,
            request.PageSize);

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
            var employeeRecords = result.Value.Select(e => new EmployeeRecord(
                e.Id,
                e.BranchId,
                e.FirstName,
                e.LastName,
                e.Email,
                e.Phone,
                e.HireDate,
                e.Position,
                e.UserId)).ToList();

            await SendAsync(
                new { 
                    status = 200, 
                    message = "Employees search completed successfully",
                    data = employeeRecords,
                    pagination = new {
                        pageNumber = request.PageNumber,
                        pageSize = request.PageSize,
                        totalResults = employeeRecords.Count
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