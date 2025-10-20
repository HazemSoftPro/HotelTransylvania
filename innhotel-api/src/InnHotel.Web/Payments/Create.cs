using InnHotel.UseCases.Payments.Create;

namespace InnHotel.Web.Payments;

public class Create(IMediator mediator) : Endpoint<CreatePaymentRequest, CreatePaymentResponse>
{
    public override void Configure()
    {
        Post("/api/payments");
        Roles("Administrator", "Manager", "Receptionist");
    }

    public override async Task HandleAsync(CreatePaymentRequest req, CancellationToken ct)
    {
        var command = new CreatePaymentCommand(
            req.ReservationId,
            req.Amount,
            req.PaymentMethod,
            req.Description
        );

        var result = await mediator.Send(command, ct);

        if (result.IsSuccess)
        {
            await SendAsync(new CreatePaymentResponse { PaymentId = result.Value }, 201, ct);
        }
        else
        {
            await SendResultAsync(Results.BadRequest(result.Errors));
        }
    }
}

public class CreatePaymentRequest
{
    public int ReservationId { get; set; }
    public decimal Amount { get; set; }
    public int PaymentMethod { get; set; }
    public string? Description { get; set; }
}

public class CreatePaymentResponse
{
    public int PaymentId { get; set; }
}