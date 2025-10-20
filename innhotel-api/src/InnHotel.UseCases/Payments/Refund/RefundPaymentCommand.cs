namespace InnHotel.UseCases.Payments.Refund;

public record RefundPaymentCommand(
    int PaymentId,
    decimal RefundAmount,
    string Reason
) : ICommand<Result<bool>>;