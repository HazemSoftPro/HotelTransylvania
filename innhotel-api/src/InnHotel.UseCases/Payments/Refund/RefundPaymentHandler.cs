using InnHotel.Core.PaymentAggregate;
using InnHotel.Core.PaymentAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace InnHotel.UseCases.Payments.Refund;

public class RefundPaymentHandler(
    IRepository<Core.PaymentAggregate.Payment> repository,
    IPaymentService paymentService,
    ILogger<RefundPaymentHandler> logger)
    : ICommandHandler<RefundPaymentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        RefundPaymentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var spec = new PaymentByIdSpec(request.PaymentId);
            var payment = await repository.FirstOrDefaultAsync(spec, cancellationToken);

            if (payment == null)
            {
                return Result<bool>.NotFound($"Payment with ID {request.PaymentId} not found");
            }

            if (payment.Status != PaymentStatus.Completed)
            {
                return Result<bool>.Error("Can only refund completed payments");
            }

            if (request.RefundAmount <= 0 || request.RefundAmount > payment.Amount)
            {
                return Result<bool>.Error("Invalid refund amount");
            }

            // Process refund through payment service
            var result = await paymentService.ProcessRefundAsync(
                payment, 
                request.RefundAmount, 
                request.Reason, 
                cancellationToken);

            if (result.Success)
            {
                payment.ProcessRefund(request.RefundAmount, request.Reason);
                await repository.UpdateAsync(payment, cancellationToken);

                logger.LogInformation(
                    "Refund processed successfully for payment {PaymentId}. Amount: {Amount}",
                    request.PaymentId, request.RefundAmount);

                return Result<bool>.Success(true);
            }
            else
            {
                logger.LogWarning(
                    "Refund processing failed for payment {PaymentId}. Error: {Error}",
                    request.PaymentId, result.ErrorMessage);

                return Result<bool>.Error(result.ErrorMessage ?? "Refund processing failed");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing refund for payment {PaymentId}", request.PaymentId);
            return Result<bool>.Error("An error occurred while processing the refund");
        }
    }
}