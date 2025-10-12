using InnHotel.UseCases.Services.Create;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Services;

/// <summary>
/// Create a new Service.
/// </summary>
/// <remarks>
/// Creates a new hotel service with the provided details.
/// </remarks>
public class Create(IMediator _mediator)
    : Endpoint<CreateServiceRequest, object>
{
    public override void Configure()
    {
        Post(CreateServiceRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin);
        Summary(s =>
        {
            s.ExampleRequest = new CreateServiceRequest
            {
                BranchId = 1,
                Name = "Spa Service",
                Price = 75.00m,
                Description = "Relaxing spa treatments and massages"
            };
        });
    }

    public override async Task HandleAsync(
        CreateServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateServiceCommand(
            request.BranchId,
            request.Name!,
            request.Price,
            request.Description);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendAsync(
                new FailureResponse(404, result.Errors.First()),
                statusCode: 404,
                cancellation: cancellationToken);
            return;
        }

        if (result.Status == ResultStatus.Error)
        {
            await SendAsync(
                new FailureResponse(400, result.Errors.First()),
                statusCode: 400,
                cancellation: cancellationToken);
            return;
        }

        if (result.IsSuccess)
        {
            var serviceRecord = new ServiceRecord(
                result.Value.Id,
                result.Value.BranchId,
                result.Value.BranchName,
                result.Value.Name,
                result.Value.Price,
                result.Value.Description);

            await SendAsync(
                new { status = 201, message = "Service created successfully", data = serviceRecord },
                statusCode: 201,
                cancellation: cancellationToken);
            return;
        }

        await SendAsync(
            new FailureResponse(500, "An unexpected error occurred."),
            statusCode: 500,
            cancellation: cancellationToken);
    }
}
