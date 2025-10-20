using FluentValidation;

namespace InnHotel.Web.Employees;

public class SearchEmployeesValidator : Validator<SearchEmployeesRequest>
{
    public SearchEmployeesValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must not exceed 100");

        When(x => x.HireDateFrom.HasValue && x.HireDateTo.HasValue, () =>
        {
            RuleFor(x => x.HireDateFrom)
                .LessThanOrEqualTo(x => x.HireDateTo)
                .WithMessage("Hire date from must be before or equal to hire date to");
        });
    }
}