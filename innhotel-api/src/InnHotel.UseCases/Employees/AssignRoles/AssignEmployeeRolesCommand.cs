namespace InnHotel.UseCases.Employees.AssignRoles;

public record AssignEmployeeRolesCommand(
    int EmployeeId,
    List<string> Roles) : ICommand<Result>;
