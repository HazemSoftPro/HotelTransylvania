using InnHotel.Core.ReservationAggregate;

namespace InnHotel.UseCases.Reservations.CheckAvailability;

public class CheckRoomAvailabilityHandler(
    IRepository<Reservation> reservationRepository,
    RoomAvailabilityService availabilityService)
    : IQueryHandler<CheckRoomAvailabilityQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(CheckRoomAvailabilityQuery request, CancellationToken cancellationToken)
    {
        // Get all reservations
        var reservations = await reservationRepository.ListAsync(cancellationToken);

        // Check availability
        var isAvailable = availabilityService.IsRoomAvailable(
            request.RoomId,
            request.CheckInDate,
            request.CheckOutDate,
            reservations,
            request.ExcludeReservationId);

        return Result<bool>.Success(isAvailable);
    }
}