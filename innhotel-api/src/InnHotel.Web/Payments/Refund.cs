using InnHotel.UseCases.Payments.Refund;

namespace InnHotel.Web.Payments;

public class Refund(IMediator mediator) : Endpoint<RefundPaymentRequest, RefundPaymentResponse>
{
    public override void Configure()
    {
        Post("/api/payments/{id}/refund");
        Roles("Administrator", "Manager");
    }

    public override async Task HandleAsync(RefundPaymentRequest req, CancellationToken ct)
    {
        var command = new RefundPaymentCommand(
            req.Id,
            req.RefundAmount,
            req.Reason
        );

        var result = await mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            await SendAsync(new RefundPaymentResponse { Success = true }, 200, ct);
        }
        else
        {
            await SendResultAsync(Results.BadRequest(result.Errors));
        }
    }
}

public class RefundPaymentRequest
{
    public int Id { get; set; }
    public decimal RefundAmount { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class RefundPaymentResponse
{
    public bool Success { get; set; }
}