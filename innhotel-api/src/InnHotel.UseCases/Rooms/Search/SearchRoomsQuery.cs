using InnHotel.Core.RoomAggregate;

namespace InnHotel.UseCases.Rooms.Search;

public record SearchRoomsQuery(
    string? SearchTerm = null,
    int? BranchId = null,
    int? RoomTypeId = null,
    RoomStatus? Status = null,
    int? Floor = null,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<Result<IEnumerable<RoomDTO>>>;
