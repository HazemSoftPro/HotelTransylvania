namespace InnHotel.UseCases.RoomTypes.Create;

public record CreateRoomTypeCommand(
    int BranchId,
    string Name,
    int Capacity,
    string? Description) : ICommand<Result<RoomTypeDTO>>;
