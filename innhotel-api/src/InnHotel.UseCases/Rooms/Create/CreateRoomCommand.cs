namespace InnHotel.UseCases.Rooms.Create;

public record CreateRoomCommand(
    int BranchId,
    int RoomTypeId,
    string RoomNumber,
    RoomStatus Status,
    int Floor,
    decimal ManualPrice) : ICommand<Result<RoomDTO>>;
