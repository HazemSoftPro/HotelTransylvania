using FluentValidation;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Validator for GetRoomTypeByIdRequest
/// </summary>
public class GetRoomTypeByIdValidator : Validator<GetRoomTypeByIdRequest>
{
    public GetRoomTypeByIdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("RoomType ID must be greater than 0");
    }
}
