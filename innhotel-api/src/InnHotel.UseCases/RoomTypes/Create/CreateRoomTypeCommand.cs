namespace InnHotel.UseCases.RoomTypes.Create;

public record CreateRoomTypeCommand(
    int BranchId,
    string Name,
    decimal BasePrice,
    int Capacity,
    string? Description) : ICommand<Result<RoomTypeDTO>>;
