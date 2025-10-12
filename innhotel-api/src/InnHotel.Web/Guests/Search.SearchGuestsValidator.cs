using FluentValidation;

namespace InnHotel.Web.Guests;

/// <summary>
/// Validator for SearchGuestsRequest
/// </summary>
public class SearchGuestsValidator : Validator<SearchGuestsRequest>
{
    public SearchGuestsValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm))
            .WithMessage("Search term cannot exceed 100 characters");
    }
}
