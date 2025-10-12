using FluentValidation;

namespace InnHotel.Web.Services;

/// <summary>
/// Validator for GetServiceByIdRequest
/// </summary>
public class GetServiceByIdValidator : Validator<GetServiceByIdRequest>
{
    public GetServiceByIdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Service ID must be greater than 0");
    }
}
