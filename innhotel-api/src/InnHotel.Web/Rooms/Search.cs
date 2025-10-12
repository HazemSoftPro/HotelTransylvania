using InnHotel.UseCases.Rooms.Search;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Rooms;

/// <summary>
/// Search Rooms with filters.
/// </summary>
/// <remarks>
/// Searches and filters rooms based on various criteria including search term, branch, room type, status, and floor.
/// </remarks>
public class Search(IMediator _mediator)
    : Endpoint<SearchRoomsRequest, object>
{
    public override void Configure()
    {
        Get(SearchRoomsRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin, AuthRoles.Employee);
        Summary(s =>
        {
            s.ExampleRequest = new SearchRoomsRequest
            {
                SearchTerm = "101",
                BranchId = 1,
                RoomTypeId = 1,
                Status = Core.RoomAggregate.RoomStatus.Available,
                Floor = 1,
                PageNumber = 1,
                PageSize = 10
            };
        });
    }

    public override async Task HandleAsync(
        SearchRoomsRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchRoomsQuery(
            request.SearchTerm,
            request.BranchId,
            request.RoomTypeId,
            request.Status,
            request.Floor,
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
            var roomRecords = result.Value.Select(r => new RoomRecord(
                r.Id,
                r.BranchId,
                r.BranchName,
                r.RoomTypeId,
                r.RoomTypeName,
                r.RoomNumber,
                r.Status,
                r.Floor)).ToList();

            await SendAsync(
                new { 
                    status = 200, 
                    message = "Rooms search completed successfully", 
                    data = roomRecords,
                    pagination = new {
                        pageNumber = request.PageNumber,
                        pageSize = request.PageSize,
                        totalResults = roomRecords.Count
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
