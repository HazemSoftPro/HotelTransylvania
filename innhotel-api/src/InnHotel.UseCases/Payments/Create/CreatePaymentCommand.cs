namespace InnHotel.UseCases.Payments.Create;

public record CreatePaymentCommand(
    int ReservationId,
    decimal Amount,
    int PaymentMethod,
    string? Description = null
) : ICommand<Result<int>>;