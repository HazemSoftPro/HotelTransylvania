using InnHotel.Core.PaymentAggregate;
using InnHotel.Core.ReservationAggregate.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InnHotel.UseCases.Payments.Create;

public class CreatePaymentHandler(
    IRepository<Core.PaymentAggregate.Payment> paymentRepository,
    IRepository<Core.ReservationAggregate.Reservation> reservationRepository,
    IPaymentService paymentService,
    IHttpContextAccessor httpContextAccessor,
    ILogger<CreatePaymentHandler> logger)
    : ICommandHandler<CreatePaymentCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            // Verify reservation exists
            var reservationSpec = new ReservationByIdSpec(request.ReservationId);
            var reservation = await reservationRepository.FirstOrDefaultAsync(reservationSpec, cancellationToken);

            if (reservation == null)
            {
                return Result<int>.NotFound($"Reservation with ID {request.ReservationId} not found");
            }

            // Get current user ID from claims
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst("sub")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Result<int>.Unauthorized();
            }

            // Create payment entity
            var payment = new Core.PaymentAggregate.Payment(
                request.ReservationId,
                request.Amount,
                (PaymentMethod)request.PaymentMethod,
                PaymentStatus.Pending,
                userId,
                request.Description
            );

            // Validate payment
            var isValid = await paymentService.ValidatePaymentAsync(payment, cancellationToken);
            if (!isValid)
            {
                return Result<int>.Error("Payment validation failed");
            }

            // Process payment
            payment.Status = PaymentStatus.Processing;
            var result = await paymentService.ProcessPaymentAsync(payment, cancellationToken);

            if (result.Success)
            {
                payment.MarkAsProcessed(result.TransactionId!, result.Provider!);
                logger.LogInformation(
                    "Payment processed successfully for reservation {ReservationId}. Transaction: {TransactionId}",
                    request.ReservationId, result.TransactionId);
            }
            else
            {
                payment.MarkAsFailed(result.ErrorMessage ?? "Payment processing failed");
                logger.LogWarning(
                    "Payment processing failed for reservation {ReservationId}. Error: {Error}",
                    request.ReservationId, result.ErrorMessage);
            }

            // Save payment
            var savedPayment = await paymentRepository.AddAsync(payment, cancellationToken);

            return result.Success
                ? Result<int>.Success(savedPayment.Id)
                : Result<int>.Error(result.ErrorMessage ?? "Payment processing failed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating payment for reservation {ReservationId}", request.ReservationId);
            return Result<int>.Error("An error occurred while processing the payment");
        }
    }
}
