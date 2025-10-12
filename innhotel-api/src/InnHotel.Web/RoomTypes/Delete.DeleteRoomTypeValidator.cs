using FluentValidation;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Validator for DeleteRoomTypeRequest
/// </summary>
public class DeleteRoomTypeValidator : Validator<DeleteRoomTypeRequest>
{
    public DeleteRoomTypeValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("RoomType ID must be greater than 0");
    }
}
