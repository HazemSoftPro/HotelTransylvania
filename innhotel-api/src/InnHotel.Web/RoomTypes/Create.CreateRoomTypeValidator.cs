using FluentValidation;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Validator for CreateRoomTypeRequest
/// </summary>
public class CreateRoomTypeValidator : Validator<CreateRoomTypeRequest>
{
    public CreateRoomTypeValidator()
    {
        RuleFor(x => x.BranchId)
            .GreaterThan(0)
            .WithMessage("Branch ID must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.BasePrice)
            .GreaterThan(0)
            .WithMessage("Base price must be greater than 0");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0")
            .LessThanOrEqualTo(20)
            .WithMessage("Capacity cannot exceed 20");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters");
    }
}
