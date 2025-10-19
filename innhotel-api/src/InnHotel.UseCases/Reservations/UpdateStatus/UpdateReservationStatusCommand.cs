namespace InnHotel.UseCases.Reservations.UpdateStatus;

public record UpdateReservationStatusCommand(
    int ReservationId,
    string NewStatus
) : ICommand<Result>;