using InnHotel.Core.RoomAggregate;
using InnHotel.Core.RoomAggregate.Specifications;

namespace InnHotel.UseCases.RoomTypes.List;

public class ListRoomTypesHandler(IReadRepository<RoomType> _repository)
    : IQueryHandler<ListRoomTypesQuery, Result<IEnumerable<RoomTypeDTO>>>
{
    public async Task<Result<IEnumerable<RoomTypeDTO>>> Handle(ListRoomTypesQuery request, CancellationToken cancellationToken)
    {
        var spec = new RoomTypeWithBranchSpec();
        var roomTypes = await _repository.ListAsync(spec, cancellationToken);

        var roomTypeDtos = roomTypes.Select(rt => new RoomTypeDTO(
            rt.Id,
            rt.BranchId,
            rt.Branch.Name,
            rt.Name,
            rt.BasePrice,
            rt.Capacity,
            rt.Description)).ToList();

        return Result.Success(roomTypeDtos.AsEnumerable());
    }
}
