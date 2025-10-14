using InnHotel.UseCases.RoomTypes.Create;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Create a new RoomType.
/// </summary>
/// <remarks>
/// Creates a new RoomType with the provided details.
/// </remarks>
public class Create(IMediator _mediator)
    : Endpoint<CreateRoomTypeRequest, object>
{
    public override void Configure()
    {
        Post(CreateRoomTypeRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin);
        Summary(s =>
        {
            s.ExampleRequest = new CreateRoomTypeRequest
            {
                BranchId = 1,
                Name = "Standard Room",
                Capacity = 2,
                Description = "A comfortable standard room with basic amenities"
            };
        });
    }

    public override async Task HandleAsync(
        CreateRoomTypeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateRoomTypeCommand(
            request.BranchId,
            request.Name!,
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
                result.Value.Capacity,
                result.Value.Description);

            await SendAsync(
                new { status = 201, message = "RoomType created successfully", data = roomTypeRecord },
                statusCode: 201,
                cancellation: cancellationToken);
            return;
        }

        await SendAsync(
            new FailureResponse(500, "An unexpected error occurred."),
            statusCode: 500,
            cancellation: cancellationToken);
    }
}
