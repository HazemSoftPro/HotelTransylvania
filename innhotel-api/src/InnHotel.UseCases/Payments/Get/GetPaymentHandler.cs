using InnHotel.Core.PaymentAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace InnHotel.UseCases.Payments.Get;

public class GetPaymentHandler(
    IRepository<Core.PaymentAggregate.Payment> repository,
    ILogger<GetPaymentHandler> logger)
    : IQueryHandler<GetPaymentQuery, Result<PaymentDTO>>
{
    public async Task<Result<PaymentDTO>> Handle(
        GetPaymentQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving payment with ID {PaymentId}", request.PaymentId);
        
        var spec = new PaymentByIdSpec(request.PaymentId);
        var payment = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (payment == null)
        {
            logger.LogWarning("Payment with ID {PaymentId} not found", request.PaymentId);
            return Result<PaymentDTO>.NotFound($"Payment with ID {request.PaymentId} not found");
        }

        var dto = new PaymentDTO
        {
            Id = payment.Id,
            ReservationId = payment.ReservationId,
            Amount = payment.Amount,
            Method = payment.Method.ToString(),
            Status = payment.Status.ToString(),
            PaymentDate = payment.PaymentDate,
            ProcessedDate = payment.ProcessedDate,
            TransactionId = payment.TransactionId,
            PaymentProvider = payment.PaymentProvider,
            IsRefunded = payment.IsRefunded,
            RefundedAmount = payment.RefundedAmount,
            RefundDate = payment.RefundDate,
            RefundReason = payment.RefundReason,
            Description = payment.Description,
            Notes = payment.Notes,
            GuestName = $"{payment.Reservation.Guest.FirstName} {payment.Reservation.Guest.LastName}"
        };

        return Result<PaymentDTO>.Success(dto);
    }
}
