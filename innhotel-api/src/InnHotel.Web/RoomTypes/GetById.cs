using InnHotel.UseCases.RoomTypes.Get;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Get a RoomType by ID.
/// </summary>
/// <remarks>
/// Retrieves a specific RoomType by its ID.
/// </remarks>
public class GetById(IMediator _mediator)
    : Endpoint<GetRoomTypeByIdRequest, object>
{
    public override void Configure()
    {
        Get(GetRoomTypeByIdRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin, AuthRoles.Employee);
    }

    public override async Task HandleAsync(
        GetRoomTypeByIdRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetRoomTypeByIdQuery(request.Id);
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
            var roomTypeRecord = new RoomTypeRecord(
                result.Value.Id,
                result.Value.BranchId,
                result.Value.BranchName,
                result.Value.Name,
                result.Value.Capacity,
                result.Value.Description);

            await SendAsync(
                new { status = 200, message = "RoomType retrieved successfully", data = roomTypeRecord },
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
