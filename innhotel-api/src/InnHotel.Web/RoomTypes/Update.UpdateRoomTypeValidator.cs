using FluentValidation;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Validator for UpdateRoomTypeRequest
/// </summary>
public class UpdateRoomTypeValidator : Validator<UpdateRoomTypeRequest>
{
    public UpdateRoomTypeValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("RoomType ID must be greater than 0");

        RuleFor(x => x.BranchId)
            .GreaterThan(0)
            .WithMessage("Branch ID must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters");



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
