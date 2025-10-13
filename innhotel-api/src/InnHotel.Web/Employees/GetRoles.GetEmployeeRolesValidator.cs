using FluentValidation;

namespace InnHotel.Web.Employees;

/// <summary>
/// Validator for GetEmployeeRolesRequest
/// </summary>
public class GetEmployeeRolesValidator : Validator<GetEmployeeRolesRequest>
{
    public GetEmployeeRolesValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Employee ID must be greater than 0");
    }
}
