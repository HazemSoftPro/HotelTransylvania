using InnHotel.UseCases.Reservations.List;
using InnHotel.Web.Common;

namespace InnHotel.Web.Reservations;

/// <summary>
/// List all Reservations
/// </summary>
public class List(IMediator _mediator)
    : Endpoint<ListReservationsRequest, PagedResponse<ReservationRecord>>
{
    public override void Configure()
    {
        Get(ListReservationsRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        ListReservationsRequest request,
        CancellationToken cancellationToken)
    {
        var query = new ListReservationsQuery(request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            var reservationRecords = result.Value.Items.Select(r => new ReservationRecord(
                r.Id,
                r.GuestId,
                r.GuestName,
                r.CheckInDate,
                r.CheckOutDate,
                r.ReservationDate,
                r.Status,
                r.TotalCost,
                r.Rooms.Select(room => new ReservationRoomRecord(
                    room.RoomId,
                    room.RoomNumber,
                    room.RoomTypeName,
                    room.PricePerNight
                )).ToList(),
                r.Services.Select(service => new ReservationServiceRecord(
                    service.ServiceId,
                    service.ServiceName,
                    service.Quantity,
                    service.UnitPrice,
                    service.TotalPrice
                )).ToList()
            )).ToList();

            var response = new PagedResponse<ReservationRecord>(
                reservationRecords,
                result.Value.TotalCount,
                request.PageNumber,
                request.PageSize
            );

            await SendOkAsync(response, cancellationToken);
        }
        else
        {
            await SendNotFoundAsync(cancellationToken);
        }
    }
}
