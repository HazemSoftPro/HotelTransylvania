using InnHotel.UseCases.Reservations.Create;
using InnHotel.Web.Common;

namespace InnHotel.Web.Reservations;

/// <summary>
/// Create a new Reservation
/// </summary>
public class Create(IMediator _mediator)
    : Endpoint<CreateReservationRequest, object>
{
    public override void Configure()
    {
        Post(CreateReservationRequest.Route);
        AllowAnonymous(); // You may want to change this to require authentication
        Summary(s =>
        {
            s.ExampleRequest = new CreateReservationRequest
            {
                GuestId = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                Rooms = new List<ReservationRoomRequest>
                {
                    new() { RoomId = 1, PricePerNight = 100 }
                },
                Services = new List<ReservationServiceRequest>
                {
                    new() { ServiceId = 1, Quantity = 2, UnitPrice = 25 }
                }
            };
        });
    }

    public override async Task HandleAsync(
        CreateReservationRequest request,
        CancellationToken cancellationToken)
    {
        // Map request to command
        var rooms = request.Rooms.Select(r => new CreateReservationRoomDto(
            r.RoomId,
            r.PricePerNight
        )).ToList();

        var services = request.Services?.Select(s => new CreateReservationServiceDto(
            s.ServiceId,
            s.Quantity,
            s.UnitPrice
        )).ToList();

        var command = new CreateReservationCommand(
            request.GuestId,
            request.CheckInDate,
            request.CheckOutDate,
            rooms,
            services
        );

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

            await SendAsync(
                reservationRecord,
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
