namespace InnHotel.UseCases.RoomTypes.List;

public record ListRoomTypesQuery(int PageNumber = 1, int PageSize = 10) : IQuery<Result<IEnumerable<RoomTypeDTO>>>;
