using InnHotel.UseCases.RoomTypes.List;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// List all RoomTypes.
/// </summary>
/// <remarks>
/// Retrieves a list of all RoomTypes with their details.
/// </remarks>
public class List(IMediator _mediator)
    : EndpointWithoutRequest<object>
{
    public override void Configure()
    {
        Get(ListRoomTypeRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin, AuthRoles.Employee);
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var query = new ListRoomTypesQuery();
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
                rt.BasePrice,
                rt.Capacity,
                rt.Description)).ToList();

            await SendAsync(
                new { status = 200, message = "RoomTypes retrieved successfully", data = roomTypeRecords },
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
