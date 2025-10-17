using InnHotel.Core.RoomAggregate;
using InnHotel.Core.RoomAggregate.Specifications;

namespace InnHotel.UseCases.RoomTypes.Search;

public class SearchRoomTypesHandler(IReadRepository<RoomType> _repository)
    : IQueryHandler<SearchRoomTypesQuery, Result<IEnumerable<RoomTypeDTO>>>
{
    public async Task<Result<IEnumerable<RoomTypeDTO>>> Handle(SearchRoomTypesQuery request, CancellationToken cancellationToken)
    {
        var spec = new RoomTypeSearchSpec(
            request.SearchTerm,
            request.BranchId,
            request.MinCapacity,
            request.MaxCapacity,
            request.PageNumber,
            request.PageSize);

        var roomTypes = await _repository.ListAsync(spec, cancellationToken);

        var roomTypeDtos = roomTypes.Select(rt => new RoomTypeDTO(
            rt.Id,
            rt.BranchId,
            rt.Branch.Name,
            rt.Name,
            rt.Capacity,
            rt.Description)).ToList();

        return Result.Success(roomTypeDtos.AsEnumerable());
    }
}