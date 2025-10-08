using InnHotel.UseCases.Reservations.GetById;
using InnHotel.Web.Common;

namespace InnHotel.Web.Reservations;

/// <summary>
/// Get a Reservation by ID
/// </summary>
public class GetById(IMediator _mediator)
    : Endpoint<GetReservationByIdRequest, object>
{
    public override void Configure()
    {
        Get(GetReservationByIdRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetReservationByIdRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetReservationByIdQuery(request.Id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        if (result.IsSuccess)
        {
            var reservationRecord = new ReservationRecord(
                result.Value.Id,
                result.Value.GuestId,
                result.Value.GuestName,
                result.Value.CheckInDate,
                result.Value.CheckOutDate,
                result.Value.ReservationDate,
                result.Value.Status,
                result.Value.TotalCost,
                result.Value.Rooms.Select(r => new ReservationRoomRecord(
                    r.RoomId,
                    r.RoomNumber,
                    r.RoomTypeName,
                    r.PricePerNight
                )).ToList(),
                result.Value.Services.Select(s => new ReservationServiceRecord(
                    s.ServiceId,
                    s.ServiceName,
                    s.Quantity,
                    s.UnitPrice,
                    s.TotalPrice
                )).ToList()
            );

            await SendOkAsync(reservationRecord, cancellationToken);
        }
        else
        {
            await SendAsync(
                new FailureResponse(500, "An error occurred while fetching the reservation."),
                statusCode: 500,
                cancellation: cancellationToken);
        }
    }
}
