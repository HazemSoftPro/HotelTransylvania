using InnHotel.UseCases.RoomTypes.Delete;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Delete a RoomType.
/// </summary>
/// <remarks>
/// Deletes an existing RoomType by its ID.
/// </remarks>
public class Delete(IMediator _mediator)
    : Endpoint<DeleteRoomTypeRequest, object>
{
    public override void Configure()
    {
        Delete(DeleteRoomTypeRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin);
    }

    public override async Task HandleAsync(
        DeleteRoomTypeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new DeleteRoomTypeCommand(request.Id);
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
                new { status = 200, message = "RoomType deleted successfully" },
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
