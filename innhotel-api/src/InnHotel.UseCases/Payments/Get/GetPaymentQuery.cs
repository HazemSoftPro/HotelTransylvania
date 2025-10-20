namespace InnHotel.UseCases.Payments.Get;

public record GetPaymentQuery(int PaymentId) : IQuery<Result<PaymentDTO>>;