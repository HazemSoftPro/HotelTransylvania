using InnHotel.Core.AuthAggregate;
using InnHotel.Core.EmployeeAggregate;
using Microsoft.AspNetCore.Identity;

namespace InnHotel.UseCases.Employees.GetRoles;

public class GetEmployeeRolesHandler(
    IReadRepository<Employee> _employeeRepository,
    UserManager<ApplicationUser> _userManager)
    : IQueryHandler<GetEmployeeRolesQuery, Result<List<string>>>
{
    public async Task<Result<List<string>>> Handle(GetEmployeeRolesQuery request, CancellationToken cancellationToken)
    {
        // Get the employee
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee == null)
        {
            return Result.NotFound($"Employee with ID {request.EmployeeId} not found.");
        }

        // Check if employee has a user account
        if (string.IsNullOrEmpty(employee.UserId))
        {
            return Result.Success(new List<string>());
        }

        // Get the user
        var user = await _userManager.FindByIdAsync(employee.UserId);
        if (user == null)
        {
            return Result.Success(new List<string>());
        }

        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);
        return Result.Success(roles.ToList());
    }
}
