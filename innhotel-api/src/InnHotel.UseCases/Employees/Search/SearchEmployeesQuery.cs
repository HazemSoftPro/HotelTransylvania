namespace InnHotel.UseCases.Employees.Search;

public record SearchEmployeesQuery(
    string? SearchTerm = null,
    int? BranchId = null,
    string? Position = null,
    DateTime? HireDateFrom = null,
    DateTime? HireDateTo = null,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<Result<IEnumerable<EmployeeDTO>>>;
