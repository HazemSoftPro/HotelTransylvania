namespace InnHotel.UseCases.Rooms.Update;

public record UpdateRoomCommand(
    int RoomId,
    int RoomTypeId,
    string RoomNumber,
    RoomStatus Status,
    int Floor,
    decimal ManualPrice) : ICommand<Result<RoomDTO>>;
