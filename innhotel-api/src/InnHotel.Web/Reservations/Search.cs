using InnHotel.UseCases.Reservations.Search;
using InnHotel.Web.Common;
using InnHotel.Core.ReservationAggregate;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Reservations;

/// <summary>
/// Search Reservations with filters.
/// </summary>
/// <remarks>
/// Searches and filters reservations based on various criteria including search term, guest, status, and date ranges.
/// </remarks>
public class Search(IMediator _mediator)
    : Endpoint<SearchReservationsRequest, object>
{
    public override void Configure()
    {
        Get(SearchReservationsRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin, AuthRoles.Employee);
        Summary(s =>
        {
            s.ExampleRequest = new SearchReservationsRequest
            {
                SearchTerm = "John",
                GuestId = 1,
                Status = ReservationStatus.Confirmed,
                CheckInDateFrom = DateTime.UtcNow,
                CheckInDateTo = DateTime.UtcNow.AddDays(7),
                PageNumber = 1,
                PageSize = 10
            };
        });
    }

    public override async Task HandleAsync(
        SearchReservationsRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchReservationsQuery(
            request.SearchTerm,
            request.GuestId,
            request.Status,
            request.CheckInDateFrom,
            request.CheckInDateTo,
            request.CheckOutDateFrom,
            request.CheckOutDateTo,
            request.PageNumber,
            request.PageSize);

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
                    message = "Reservations search completed successfully", 
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