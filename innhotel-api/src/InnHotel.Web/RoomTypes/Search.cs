using InnHotel.UseCases.RoomTypes.Search;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Search Room Types with filters.
/// </summary>
/// <remarks>
/// Searches and filters room types based on various criteria including search term, branch, and capacity range.
/// </remarks>
public class Search(IMediator _mediator)
    : Endpoint<SearchRoomTypesRequest, object>
{
    public override void Configure()
    {
        Get(SearchRoomTypesRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin, AuthRoles.Employee);
        Summary(s =>
        {
            s.ExampleRequest = new SearchRoomTypesRequest
            {
                SearchTerm = "Deluxe",
                BranchId = 1,
                MinCapacity = 2,
                MaxCapacity = 4,
                PageNumber = 1,
                PageSize = 10
            };
        });
    }

    public override async Task HandleAsync(
        SearchRoomTypesRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchRoomTypesQuery(
            request.SearchTerm,
            request.BranchId,
            request.MinCapacity,
            request.MaxCapacity,
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
            var roomTypeRecords = result.Value.Select(rt => new RoomTypeRecord(
                rt.Id,
                rt.BranchId,
                rt.BranchName,
                rt.Name,
                rt.Capacity,
                rt.Description)).ToList();

            await SendAsync(
                new { 
                    status = 200, 
                    message = "Room types search completed successfully", 
                    data = roomTypeRecords,
                    pagination = new {
                        pageNumber = request.PageNumber,
                        pageSize = request.PageSize,
                        totalResults = roomTypeRecords.Count
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