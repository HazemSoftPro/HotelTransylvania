namespace InnHotel.UseCases.Employees.Create;

public record CreateEmployeeCommand(
    int BranchId,
    string FirstName,
    string LastName,
    string? Email,
    string? Phone,
    DateOnly HireDate,
    string Position,
    string? UserId
) : ICommand<Result<EmployeeDTO>>;
