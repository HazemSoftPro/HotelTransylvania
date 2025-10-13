using FluentValidation;

namespace InnHotel.Web.Guests;

/// <summary>
/// Validator for GetGuestHistoryRequest
/// </summary>
public class GetGuestHistoryValidator : Validator<GetGuestHistoryRequest>
{
    public GetGuestHistoryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Guest ID must be greater than 0");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100");
    }
}
