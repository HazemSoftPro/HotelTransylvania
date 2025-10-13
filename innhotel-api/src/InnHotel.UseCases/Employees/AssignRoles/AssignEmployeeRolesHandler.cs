using InnHotel.Core.AuthAggregate;
using InnHotel.Core.EmployeeAggregate;
using Microsoft.AspNetCore.Identity;

namespace InnHotel.UseCases.Employees.AssignRoles;

public class AssignEmployeeRolesHandler(
    IReadRepository<Employee> _employeeRepository,
    UserManager<ApplicationUser> _userManager)
    : ICommandHandler<AssignEmployeeRolesCommand, Result>
{
    public async Task<Result> Handle(AssignEmployeeRolesCommand request, CancellationToken cancellationToken)
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
            return Result.Error("Employee must have a user account to assign roles.");
        }

        // Get the user
        var user = await _userManager.FindByIdAsync(employee.UserId);
        if (user == null)
        {
            return Result.Error("User account not found for this employee.");
        }

        // Validate that all requested roles exist
        var validRoles = new List<string> { Roles.Admin, Roles.SuperAdmin, Roles.Employee, Roles.Receptionist, Roles.Housekeeping };
        var invalidRoles = request.Roles.Where(r => !validRoles.Contains(r)).ToList();
        if (invalidRoles.Any())
        {
            return Result.Error($"Invalid roles: {string.Join(", ", invalidRoles)}");
        }

        // Get current roles
        var currentRoles = await _userManager.GetRolesAsync(user);

        // Remove all current roles
        if (currentRoles.Any())
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return Result.Error($"Failed to remove current roles: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
            }
        }

        // Add new roles
        if (request.Roles.Any())
        {
            var addResult = await _userManager.AddToRolesAsync(user, request.Roles);
            if (!addResult.Succeeded)
            {
                return Result.Error($"Failed to assign roles: {string.Join(", ", addResult.Errors.Select(e => e.Description))}");
            }
        }

        return Result.Success();
    }
}
