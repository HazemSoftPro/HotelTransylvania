namespace InnHotel.UseCases.RoomTypes.Get;

public record GetRoomTypeByIdQuery(int Id) : IQuery<Result<RoomTypeDTO>>;
