using FluentValidation;

namespace InnHotel.Web.Services;

/// <summary>
/// Validator for DeleteServiceRequest
/// </summary>
public class DeleteServiceValidator : Validator<DeleteServiceRequest>
{
    public DeleteServiceValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Service ID must be greater than 0");
    }
}
