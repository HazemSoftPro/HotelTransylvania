using InnHotel.Core.BranchAggregate;
using InnHotel.Core.ServiceAggregate;

namespace InnHotel.UseCases.Services.Create;

public class CreateServiceHandler(IRepository<Service> _serviceRepository, IRepository<Branch> _branchRepository)
    : ICommandHandler<CreateServiceCommand, Result<ServiceDTO>>
{
    public async Task<Result<ServiceDTO>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        // Check if branch exists
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
        if (branch == null)
        {
            return Result.NotFound($"Branch with ID {request.BranchId} not found.");
        }

        // Create new service
        var service = new Service(
            request.BranchId,
            request.Name,
            request.Price,
            request.Description);

        var createdService = await _serviceRepository.AddAsync(service, cancellationToken);
        await _serviceRepository.SaveChangesAsync(cancellationToken);

        var serviceDto = new ServiceDTO(
            createdService.Id,
            createdService.BranchId,
            branch.Name,
            createdService.Name,
            createdService.Price,
            createdService.Description);

        return Result.Success(serviceDto);
    }
}
