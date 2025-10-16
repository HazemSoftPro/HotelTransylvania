namespace InnHotel.UseCases.RoomTypes.Search;

public record SearchRoomTypesQuery(
    string? SearchTerm = null,
    int? BranchId = null,
    int? MinCapacity = null,
    int? MaxCapacity = null,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<Result<IEnumerable<RoomTypeDTO>>>;