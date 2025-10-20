using FluentValidation;

namespace InnHotel.Web.Reservations;

public class SearchReservationsValidator : Validator<SearchReservationsRequest>
{
    public SearchReservationsValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must not exceed 100");

        When(x => x.CheckInDateFrom.HasValue && x.CheckInDateTo.HasValue, () =>
        {
            RuleFor(x => x.CheckInDateFrom)
                .LessThanOrEqualTo(x => x.CheckInDateTo)
                .WithMessage("Check-in date from must be before or equal to check-in date to");
        });

        When(x => x.CheckOutDateFrom.HasValue && x.CheckOutDateTo.HasValue, () =>
        {
            RuleFor(x => x.CheckOutDateFrom)
                .LessThanOrEqualTo(x => x.CheckOutDateTo)
                .WithMessage("Check-out date from must be before or equal to check-out date to");
        });
    }
}
