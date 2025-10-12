namespace InnHotel.UseCases.RoomTypes.Update;

public record UpdateRoomTypeCommand(
    int Id,
    int BranchId,
    string Name,
    decimal BasePrice,
    int Capacity,
    string? Description) : ICommand<Result<RoomTypeDTO>>;
