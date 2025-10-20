using InnHotel.Core.EmployeeAggregate;
using InnHotel.Core.EmployeeAggregate.Specifications;

namespace InnHotel.UseCases.Employees.Search;

public record EmployeeDTO(
    int Id,
    int BranchId,
    string FirstName,
    string LastName,
    DateOnly HireDate,
    string Position,
    string? UserId);

public class SearchEmployeesHandler(IReadRepository<Employee> _repository)
    : IQueryHandler<SearchEmployeesQuery, Result<IEnumerable<EmployeeDTO>>>
{
    public async Task<Result<IEnumerable<EmployeeDTO>>> Handle(SearchEmployeesQuery request, CancellationToken cancellationToken)
    {
        var spec = new EmployeeSearchSpec(
            request.SearchTerm,
            request.BranchId,
            request.Position,
            request.HireDateFrom,
            request.HireDateTo,
            request.PageNumber,
            request.PageSize);

        var employees = await _repository.ListAsync(spec, cancellationToken);

        var employeeDtos = employees.Select(e => new EmployeeDTO(
            e.Id,
            e.BranchId,
            e.FirstName,
            e.LastName,
            e.HireDate,
            e.Position,
            e.UserId)).ToList();

        return Result.Success(employeeDtos.AsEnumerable());
    }
}
