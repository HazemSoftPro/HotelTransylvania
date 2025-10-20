using FluentValidation;

namespace InnHotel.Web.RoomTypes;

public class SearchRoomTypesValidator : Validator<SearchRoomTypesRequest>
{
    public SearchRoomTypesValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must not exceed 100");

        When(x => x.MinCapacity.HasValue && x.MaxCapacity.HasValue, () =>
        {
            RuleFor(x => x.MinCapacity)
                .LessThanOrEqualTo(x => x.MaxCapacity)
                .WithMessage("Minimum capacity must be less than or equal to maximum capacity");
        });

        When(x => x.MinCapacity.HasValue, () =>
        {
            RuleFor(x => x.MinCapacity)
                .GreaterThan(0)
                .WithMessage("Minimum capacity must be greater than 0");
        });

        When(x => x.MaxCapacity.HasValue, () =>
        {
            RuleFor(x => x.MaxCapacity)
                .GreaterThan(0)
                .WithMessage("Maximum capacity must be greater than 0");
        });
    }
}
