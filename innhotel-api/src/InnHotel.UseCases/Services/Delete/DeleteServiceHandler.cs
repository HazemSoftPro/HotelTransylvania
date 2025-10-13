using InnHotel.Core.ServiceAggregate;

namespace InnHotel.UseCases.Services.Delete;

public class DeleteServiceHandler(IRepository<Service> _repository)
    : ICommandHandler<DeleteServiceCommand, Result>
{
    public async Task<Result> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (service == null)
        {
            return Result.NotFound($"Service with ID {request.Id} not found.");
        }

        await _repository.DeleteAsync(service, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
