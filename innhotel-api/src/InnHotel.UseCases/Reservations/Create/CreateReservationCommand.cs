using MediatR;

namespace InnHotel.UseCases.Reservations.Create;

/// <summary>
/// Create a new Reservation command
/// </summary>
public record CreateReservationCommand(
    int GuestId,
    DateOnly CheckInDate,
    DateOnly CheckOutDate,
    List<CreateReservationRoomDto> Rooms,
    List<CreateReservationServiceDto>? Services
) : IRequest<Result<ReservationDto>>;

public record CreateReservationRoomDto(
    int RoomId,
    decimal PricePerNight
);

public record CreateReservationServiceDto(
    int ServiceId,
    int Quantity,
    decimal UnitPrice
);
