using InnHotel.Core.ServiceAggregate;
using InnHotel.Core.ServiceAggregate.Specifications;

namespace InnHotel.UseCases.Services.Search;

public class SearchServicesHandler(IReadRepository<Service> _repository)
    : IQueryHandler<SearchServicesQuery, Result<IEnumerable<ServiceDTO>>>
{
    public async Task<Result<IEnumerable<ServiceDTO>>> Handle(SearchServicesQuery request, CancellationToken cancellationToken)
    {
        var spec = new ServiceSearchSpec(
            request.SearchTerm,
            request.BranchId,
            request.MinPrice,
            request.MaxPrice,
            request.PageNumber,
            request.PageSize);

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