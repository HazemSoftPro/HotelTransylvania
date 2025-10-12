using InnHotel.UseCases.RoomTypes.Update;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Update a RoomType.
/// </summary>
/// <remarks>
/// Updates an existing RoomType with the provided details.
/// </remarks>
public class Update(IMediator _mediator)
    : Endpoint<UpdateRoomTypeRequest, object>
{
    public override void Configure()
    {
        Put(UpdateRoomTypeRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin);
        Summary(s =>
        {
            s.ExampleRequest = new UpdateRoomTypeRequest
            {
                Id = 1,
                BranchId = 1,
                Name = "Deluxe Room",
                BasePrice = 150.00m,
                Capacity = 3,
                Description = "A luxurious deluxe room with premium amenities"
            };
        });
    }

    public override async Task HandleAsync(
        UpdateRoomTypeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoomTypeCommand(
            request.Id,
            request.BranchId,
            request.Name!,
            request.BasePrice,
            request.Capacity,
            request.Description);

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
            var roomTypeRecord = new RoomTypeRecord(
                result.Value.Id,
                result.Value.BranchId,
                result.Value.BranchName,
                result.Value.Name,
                result.Value.BasePrice,
                result.Value.Capacity,
                result.Value.Description);

            await SendAsync(
                new { status = 200, message = "RoomType updated successfully", data = roomTypeRecord },
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
