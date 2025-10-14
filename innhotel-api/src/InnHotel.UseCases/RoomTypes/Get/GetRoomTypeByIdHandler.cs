using InnHotel.Core.RoomAggregate;
using InnHotel.Core.RoomAggregate.Specifications;

namespace InnHotel.UseCases.RoomTypes.Get;

public class GetRoomTypeByIdHandler(IReadRepository<RoomType> _repository)
    : IQueryHandler<GetRoomTypeByIdQuery, Result<RoomTypeDTO>>
{
    public async Task<Result<RoomTypeDTO>> Handle(GetRoomTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new RoomTypeByIdWithBranchSpec(request.Id);
        var roomType = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (roomType == null)
        {
            return Result.NotFound($"RoomType with ID {request.Id} not found.");
        }

        var roomTypeDto = new RoomTypeDTO(
            roomType.Id,
            roomType.BranchId,
            roomType.Branch.Name,
            roomType.Name,
            roomType.Capacity,
            roomType.Description);

        return Result.Success(roomTypeDto);
    }
}
