namespace InnHotel.UseCases.Employees.Update;

public record UpdateEmployeeCommand(
    int EmployeeId,
    int    BranchId,
    string FirstName,
    string LastName,
    string? Email,
    string? Phone,
    DateOnly HireDate,
    string Position,
    string? UserId
) : ICommand<Result<EmployeeDTO>>;
