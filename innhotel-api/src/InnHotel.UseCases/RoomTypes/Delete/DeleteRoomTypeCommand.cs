namespace InnHotel.UseCases.RoomTypes.Delete;

public record DeleteRoomTypeCommand(int Id) : ICommand<Result>;
