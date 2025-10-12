using InnHotel.Core.BranchAggregate;
using InnHotel.Core.ServiceAggregate;
using InnHotel.Core.ServiceAggregate.Specifications;

namespace InnHotel.UseCases.Services.Update;

public class UpdateServiceHandler(IRepository<Service> _serviceRepository, IRepository<Branch> _branchRepository)
    : ICommandHandler<UpdateServiceCommand, Result<ServiceDTO>>
{
    public async Task<Result<ServiceDTO>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        // Check if service exists
        var spec = new ServiceByIdWithBranchSpec(request.Id);
        var service = await _serviceRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (service == null)
        {
            return Result.NotFound($"Service with ID {request.Id} not found.");
        }

        // Check if branch exists (if branch is being changed)
        if (service.BranchId != request.BranchId)
        {
            var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
            if (branch == null)
            {
                return Result.NotFound($"Branch with ID {request.BranchId} not found.");
            }
        }

        // Update service details
        service.UpdateDetails(
            request.BranchId,
            request.Name,
            request.Price,
            request.Description);

        await _serviceRepository.UpdateAsync(service, cancellationToken);
        await _serviceRepository.SaveChangesAsync(cancellationToken);

        // Get updated service with branch info
        var updatedSpec = new ServiceByIdWithBranchSpec(request.Id);
        var updatedService = await _serviceRepository.FirstOrDefaultAsync(updatedSpec, cancellationToken);

        var serviceDto = new ServiceDTO(
            updatedService!.Id,
            updatedService.BranchId,
            updatedService.Branch.Name,
            updatedService.Name,
            updatedService.Price,
            updatedService.Description);

        return Result.Success(serviceDto);
    }
}
