using InnHotel.UseCases.Rooms.Update;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Rooms;

/// <summary>
/// Update an existing Room.
/// </summary>
/// <remarks>
/// Updates an existing Room with the provided details.
/// </remarks>
public class Update(IMediator _mediator)
    : Endpoint<UpdateRoomRequest, object>
{
    public override void Configure()
    {
        Put(UpdateRoomRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin);
        Summary(s =>
        {
            s.ExampleRequest = new UpdateRoomRequest
            {
                RoomId = 1,
                RoomTypeId = 1,
                RoomNumber = "101",
                Status = 0,
                Floor = 1,
                PriceOverride = null
            };
        });
    }

    public override async Task HandleAsync(
        UpdateRoomRequest request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Update request: RoomId={request.RoomId}, PriceOverride={request.PriceOverride}"); // Debug log

        var command = new UpdateRoomCommand(
            request.RoomId,
            request.RoomTypeId,
            request.RoomNumber!,
            (RoomStatus)request.Status,
            request.Floor,
            request.PriceOverride);

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
            var roomRecord = new RoomRecord(
                result.Value.Id,
                result.Value.BranchId,
                result.Value.BranchName,
                result.Value.RoomTypeId,
                result.Value.RoomTypeName,
                result.Value.BasePrice,
                result.Value.Capacity,
                result.Value.RoomNumber,
                result.Value.Status,
                result.Value.Floor,
                result.Value.PriceOverride);

            await SendAsync(
                new { status = 200, message = "Room updated successfully", data = roomRecord },
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
