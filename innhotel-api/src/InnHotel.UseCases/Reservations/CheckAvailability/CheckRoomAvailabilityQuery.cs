namespace InnHotel.UseCases.Reservations.CheckAvailability;

public record CheckRoomAvailabilityQuery(
    int RoomId,
    DateOnly CheckInDate,
    DateOnly CheckOutDate,
    int? ExcludeReservationId = null
) : IQuery<Result<bool>>;