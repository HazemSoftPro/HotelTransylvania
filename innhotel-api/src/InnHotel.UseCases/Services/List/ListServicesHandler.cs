using InnHotel.Core.ServiceAggregate;
using InnHotel.Core.ServiceAggregate.Specifications;

namespace InnHotel.UseCases.Services.List;

public class ListServicesHandler(IReadRepository<Service> _repository)
    : IQueryHandler<ListServicesQuery, Result<IEnumerable<ServiceDTO>>>
{
    public async Task<Result<IEnumerable<ServiceDTO>>> Handle(ListServicesQuery request, CancellationToken cancellationToken)
    {
        var spec = new ServiceWithBranchSpec();
        var services = await _repository.ListAsync(spec, cancellationToken);

        var serviceDtos = services.Select(s => new ServiceDTO(
            s.Id,
            s.BranchId,
            s.Branch.Name,
            s.Name,
            s.Price,
            s.Description)).ToList();

        return Result.Success(serviceDtos.AsEnumerable());
    }
}
