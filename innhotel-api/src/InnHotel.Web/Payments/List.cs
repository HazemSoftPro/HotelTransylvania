using InnHotel.UseCases.Payments;
using InnHotel.UseCases.Payments.List;

namespace InnHotel.Web.Payments;

public class List(IMediator mediator) : Endpoint<ListPaymentsRequest, IEnumerable<PaymentDTO>>
{
    public override void Configure()
    {
        Get("/api/payments");
        Roles("Administrator", "Manager", "Receptionist", "Accountant");
    }

    public override async Task HandleAsync(ListPaymentsRequest req, CancellationToken ct)
    {
        var query = new ListPaymentsQuery(
            req.Status,
            req.StartDate,
            req.EndDate,
            req.ReservationId,
            req.Skip,
            req.Take
        );

        var result = await mediator.Send(query, ct);

        if (result.IsSuccess)
        {
            await SendAsync(result.Value, 200, ct);
        }
        else
        {
            await SendResultAsync(Results.BadRequest(result.Errors));
        }
    }
}

public class ListPaymentsRequest
{
    public int? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? ReservationId { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 20;
}