using InnHotel.UseCases.Services.Update;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Services;

/// <summary>
/// Update a Service.
/// </summary>
/// <remarks>
/// Updates an existing hotel service with the provided details.
/// </remarks>
public class Update(IMediator _mediator)
    : Endpoint<UpdateServiceRequest, object>
{
    public override void Configure()
    {
        Put(UpdateServiceRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin);
        Summary(s =>
        {
            s.ExampleRequest = new UpdateServiceRequest
            {
                Id = 1,
                BranchId = 1,
                Name = "Premium Spa Service",
                Price = 95.00m,
                Description = "Luxury spa treatments with premium amenities"
            };
        });
    }

    public override async Task HandleAsync(
        UpdateServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateServiceCommand(
            request.Id,
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
                new { status = 200, message = "Service updated successfully", data = serviceRecord },
                statusCode: 200,
                cancellation: cancellationToken);
            return;
        }

        await SendAsync(
            new FailureResponse(500, "An unexpected error occurred."),
            statusCode: 500,
            cancellation: cancellationToken);
    }
}
