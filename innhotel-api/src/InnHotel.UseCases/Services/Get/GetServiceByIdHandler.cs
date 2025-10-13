using InnHotel.Core.ServiceAggregate;
using InnHotel.Core.ServiceAggregate.Specifications;

namespace InnHotel.UseCases.Services.Get;

public class GetServiceByIdHandler(IReadRepository<Service> _repository)
    : IQueryHandler<GetServiceByIdQuery, Result<ServiceDTO>>
{
    public async Task<Result<ServiceDTO>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new ServiceByIdWithBranchSpec(request.Id);
        var service = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (service == null)
        {
            return Result.NotFound($"Service with ID {request.Id} not found.");
        }

        var serviceDto = new ServiceDTO(
            service.Id,
            service.BranchId,
            service.Branch.Name,
            service.Name,
            service.Price,
            service.Description);

        return Result.Success(serviceDto);
    }
}
