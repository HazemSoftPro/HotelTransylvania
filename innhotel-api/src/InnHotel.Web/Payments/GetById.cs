using InnHotel.UseCases.Payments;
using InnHotel.UseCases.Payments.Get;

namespace InnHotel.Web.Payments;

public class GetById(IMediator mediator) : Endpoint<GetPaymentRequest, PaymentDTO>
{
    public override void Configure()
    {
        Get("/api/payments/{id}");
        Roles("Administrator", "Manager", "Receptionist", "Accountant");
    }

    public override async Task HandleAsync(GetPaymentRequest req, CancellationToken ct)
    {
        var query = new GetPaymentQuery(req.Id);
        var result = await mediator.Send(query, ct);

        if (result.IsSuccess)
        {
            await SendAsync(result.Value, 200, ct);
        }
        else
        {
            await SendNotFoundAsync(ct);
        }
    }
}

public class GetPaymentRequest
{
    public int Id { get; set; }
}