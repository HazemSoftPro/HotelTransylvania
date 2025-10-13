using InnHotel.Core.GuestAggregate;
using InnHotel.Core.GuestAggregate.Specifications;
using InnHotel.Core.ReservationAggregate;
using InnHotel.UseCases.Reservations;

namespace InnHotel.UseCases.Guests.GetHistory;

public class GetGuestHistoryHandler(
    IReadRepository<Guest> _guestRepository,
    IReadRepository<Reservation> _reservationRepository)
    : IQueryHandler<GetGuestHistoryQuery, Result<IEnumerable<ReservationDto>>>
{
    public async Task<Result<IEnumerable<ReservationDto>>> Handle(GetGuestHistoryQuery request, CancellationToken cancellationToken)
    {
        // First verify the guest exists
        var guest = await _guestRepository.GetByIdAsync(request.GuestId, cancellationToken);
        if (guest == null)
        {
            return Result.NotFound($"Guest with ID {request.GuestId} not found.");
        }

        // Get guest's reservation history
        var spec = new GuestHistorySpec(request.GuestId, request.PageNumber, request.PageSize);
        var reservations = await _reservationRepository.ListAsync(spec, cancellationToken);

        var reservationDtos = reservations.Select(r =>
        {
            var firstRoom = r.Rooms.FirstOrDefault();
            return new ReservationDto
            {
                Id = r.Id,
                GuestId = r.GuestId,
                GuestName = $"{r.Guest.FirstName} {r.Guest.LastName}",
                BranchId = firstRoom?.Room?.BranchId,
                BranchName = firstRoom?.Room?.Branch?.Name ?? "Unknown",
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                ReservationDate = r.ReservationDate,
                Status = r.Status,
                TotalCost = r.TotalCost,
                Rooms = r.Rooms.Select(rr => new ReservationRoomDto
                {
                    RoomId = rr.RoomId,
                    RoomNumber = rr.Room?.RoomNumber ?? "Unknown",
                    RoomTypeName = rr.Room?.RoomType?.Name ?? "Unknown",
                    PricePerNight = rr.PricePerNight
                }).ToList(),
                Services = r.Services.Select(rs => new ReservationServiceDto
                {
                    ServiceId = rs.ServiceId,
                    ServiceName = rs.Service?.Name ?? "Service",
                    Quantity = rs.Quantity,
                    UnitPrice = rs.UnitPrice
                }).ToList()
            };
        }).ToList();

        return Result.Success(reservationDtos.AsEnumerable());
    }
}
