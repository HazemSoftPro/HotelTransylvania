using Ardalis.Specification;

namespace InnHotel.Core.ServiceAggregate.Specifications;

public sealed class ServiceWithBranchSpec : Specification<Service>
{
    public ServiceWithBranchSpec()
    {
        Query.Include(s => s.Branch);
    }
}

public sealed class ServiceByIdWithBranchSpec : Specification<Service>
{
    public ServiceByIdWithBranchSpec(int id)
    {
        Query
            .Where(s => s.Id == id)
            .Include(s => s.Branch);
    }
}
