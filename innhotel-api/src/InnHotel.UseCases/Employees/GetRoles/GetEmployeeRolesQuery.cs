namespace InnHotel.UseCases.Employees.GetRoles;

public record GetEmployeeRolesQuery(int EmployeeId) : IQuery<Result<List<string>>>;
