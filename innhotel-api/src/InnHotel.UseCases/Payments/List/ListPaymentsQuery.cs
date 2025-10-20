namespace InnHotel.UseCases.Payments.List;

public record ListPaymentsQuery(
    int? Status = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    int? ReservationId = null,
    int Skip = 0,
    int Take = 20
) : IQuery<Result<IEnumerable<PaymentDTO>>>;