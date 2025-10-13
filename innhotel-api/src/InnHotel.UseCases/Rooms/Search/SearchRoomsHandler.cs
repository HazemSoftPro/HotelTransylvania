using InnHotel.Core.RoomAggregate;
using InnHotel.Core.RoomAggregate.Specifications;

namespace InnHotel.UseCases.Rooms.Search;

public class SearchRoomsHandler(IReadRepository<Room> _repository)
    : IQueryHandler<SearchRoomsQuery, Result<IEnumerable<RoomDTO>>>
{
    public async Task<Result<IEnumerable<RoomDTO>>> Handle(SearchRoomsQuery request, CancellationToken cancellationToken)
    {
        var spec = new RoomSearchSpec(
            request.SearchTerm,
            request.BranchId,
            request.RoomTypeId,
            request.Status,
            request.Floor,
            request.PageNumber,
            request.PageSize);

        var rooms = await _repository.ListAsync(spec, cancellationToken);

        var roomDtos = rooms.Select(r => new RoomDTO(
            r.Id,
            r.BranchId,
            r.Branch.Name,
            r.RoomTypeId,
            r.RoomType.Name,
            r.RoomType.BasePrice,
            r.RoomType.Capacity,
            r.RoomNumber,
            r.Status,
            r.Floor)).ToList();

        return Result.Success(roomDtos.AsEnumerable());
    }
}
