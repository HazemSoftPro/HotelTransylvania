using InnHotel.UseCases.Guests.GetHistory;
using InnHotel.Web.Common;
using InnHotel.Web.Reservations;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Guests;

/// <summary>
/// </summary>
/// <remarks>
/// Retrieves the reservation history for a specific guest with pagination support.
/// </remarks>
public class GetHistory(IMediator _mediator)
    : Endpoint<GetGuestHistoryRequest, object>
{
    public override void Configure()
    {
        Get(GetGuestHistoryRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin, AuthRoles.Employee);
        Summary(s =>
        {
            s.ExampleRequest = new GetGuestHistoryRequest
            {
                Id = 1,
                PageNumber = 1,
                PageSize = 10
            };
        });
    }

    public override async Task HandleAsync(
        GetGuestHistoryRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetGuestHistoryQuery(
            request.Id,
            request.PageNumber,
            request.PageSize);

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
            var reservationRecords = result.Value.Select(r => new ReservationRecord(
                r.Id,
                r.GuestId,
                r.GuestName,
                r.BranchId,
                r.BranchName,
                r.CheckInDate,
                r.CheckOutDate,
                r.ReservationDate,
                r.Status,
                r.TotalCost,
                r.Rooms.Select(room => new ReservationRoomRecord(
                    room.RoomId,
                    room.RoomNumber,
                    room.RoomTypeName,
                    room.PricePerNight)).ToList(),
                r.Services.Select(service => new ReservationServiceRecord(
                    service.ServiceId,
                    service.ServiceName,
                    service.Quantity,
                    service.UnitPrice,
                    service.TotalPrice)).ToList())).ToList();

            await SendAsync(
                new { 
                    status = 200, 
                    message = "Guest history retrieved successfully", 
                    data = reservationRecords,
                    pagination = new {
                        pageNumber = request.PageNumber,
                        pageSize = request.PageSize,
                        totalResults = reservationRecords.Count
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
