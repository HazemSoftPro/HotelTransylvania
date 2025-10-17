using InnHotel.UseCases.Services.Search;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Services;

/// <summary>
/// Search Services with filters.
/// </summary>
/// <remarks>
/// Searches and filters services based on various criteria including search term, branch, and price range.
/// </remarks>
public class Search(IMediator _mediator)
    : Endpoint<SearchServicesRequest, object>
{
    public override void Configure()
    {
        Get(SearchServicesRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin, AuthRoles.Employee);
        Summary(s =>
        {
            s.ExampleRequest = new SearchServicesRequest
            {
                SearchTerm = "Spa",
                BranchId = 1,
                MinPrice = 10.00m,
                MaxPrice = 100.00m,
                PageNumber = 1,
                PageSize = 10
            };
        });
    }

    public override async Task HandleAsync(
        SearchServicesRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchServicesQuery(
            request.SearchTerm,
            request.BranchId,
            request.MinPrice,
            request.MaxPrice,
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
            var serviceRecords = result.Value.Select(s => new ServiceRecord(
                s.Id,
                s.BranchId,
                s.BranchName,
                s.Name,
                s.Price,
                s.Description)).ToList();

            await SendAsync(
                new { 
                    status = 200, 
                    message = "Services search completed successfully", 
                    data = serviceRecords,
                    pagination = new {
                        pageNumber = request.PageNumber,
                        pageSize = request.PageSize,
                        totalResults = serviceRecords.Count
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