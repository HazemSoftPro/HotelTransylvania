using FluentValidation;
using InnHotel.Core.AuthAggregate;

namespace InnHotel.Web.Employees;

/// <summary>
/// Validator for AssignEmployeeRolesRequest
/// </summary>
public class AssignEmployeeRolesValidator : Validator<AssignEmployeeRolesRequest>
{
    public AssignEmployeeRolesValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Employee ID must be greater than 0");

        RuleFor(x => x.Roles)
            .NotNull()
            .WithMessage("Roles list cannot be null");

        RuleForEach(x => x.Roles)
            .Must(BeValidRole)
            .WithMessage("Invalid role. Valid roles are: Admin, SuperAdmin, Employee, Receptionist, Housekeeping");
    }

    private static bool BeValidRole(string role)
    {
        var validRoles = new[] { Roles.Admin, Roles.SuperAdmin, Roles.Employee, Roles.Receptionist, Roles.Housekeeping };
        return validRoles.Contains(role);
    }
}
