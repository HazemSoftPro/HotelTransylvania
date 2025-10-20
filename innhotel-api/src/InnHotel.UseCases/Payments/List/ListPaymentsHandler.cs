using InnHotel.Core.PaymentAggregate;
using InnHotel.Core.PaymentAggregate.Specifications;
using Microsoft.Extensions.Logging;

namespace InnHotel.UseCases.Payments.List;

public class ListPaymentsHandler(
    IRepository<Core.PaymentAggregate.Payment> repository,
    ILogger<ListPaymentsHandler> logger)
    : IQueryHandler<ListPaymentsQuery, Result<IEnumerable<PaymentDTO>>>
{
    public async Task<Result<IEnumerable<PaymentDTO>>> Handle(
        ListPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Listing payments with filters - Status: {Status}, StartDate: {StartDate}, EndDate: {EndDate}", 
            request.Status, request.StartDate, request.EndDate);
            
        PaymentStatus? status = null;
        if (request.Status.HasValue)
        {
            status = (PaymentStatus)request.Status.Value;
        }

        var spec = new PaymentSearchSpec(
            status,
            request.StartDate,
            request.EndDate,
            request.ReservationId,
            request.Skip,
            request.Take
        );

        var payments = await repository.ListAsync(spec, cancellationToken);

        var dtos = payments.Select(p => new PaymentDTO
        {
            Id = p.Id,
            ReservationId = p.ReservationId,
            Amount = p.Amount,
            Method = p.Method.ToString(),
            Status = p.Status.ToString(),
            PaymentDate = p.PaymentDate,
            ProcessedDate = p.ProcessedDate,
            TransactionId = p.TransactionId,
            PaymentProvider = p.PaymentProvider,
            IsRefunded = p.IsRefunded,
            RefundedAmount = p.RefundedAmount,
            RefundDate = p.RefundDate,
            RefundReason = p.RefundReason,
            Description = p.Description,
            Notes = p.Notes,
            GuestName = $"{p.Reservation.Guest.FirstName} {p.Reservation.Guest.LastName}"
        });

        return Result<IEnumerable<PaymentDTO>>.Success(dtos);
    }
}
