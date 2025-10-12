using FluentValidation;

namespace InnHotel.Web.Rooms;

/// <summary>
/// Validator for SearchRoomsRequest
/// </summary>
public class SearchRoomsValidator : Validator<SearchRoomsRequest>
{
    public SearchRoomsValidator()
    {
        RuleFor(x => x.BranchId)
            .GreaterThan(0)
            .When(x => x.BranchId.HasValue)
            .WithMessage("Branch ID must be greater than 0");

        RuleFor(x => x.RoomTypeId)
            .GreaterThan(0)
            .When(x => x.RoomTypeId.HasValue)
            .WithMessage("Room Type ID must be greater than 0");

        RuleFor(x => x.Floor)
            .GreaterThan(0)
            .When(x => x.Floor.HasValue)
            .WithMessage("Floor must be greater than 0");

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
