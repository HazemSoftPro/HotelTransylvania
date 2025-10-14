namespace InnHotel.UseCases.RoomTypes.Update;

public record UpdateRoomTypeCommand(
    int Id,
    int BranchId,
    string Name,
    int Capacity,
    string? Description) : ICommand<Result<RoomTypeDTO>>;
