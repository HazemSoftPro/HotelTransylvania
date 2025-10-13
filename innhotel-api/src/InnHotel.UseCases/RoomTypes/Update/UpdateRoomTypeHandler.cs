using InnHotel.Core.BranchAggregate;
using InnHotel.Core.RoomAggregate;
using InnHotel.Core.RoomAggregate.Specifications;

namespace InnHotel.UseCases.RoomTypes.Update;

public class UpdateRoomTypeHandler(IRepository<RoomType> _roomTypeRepository, IRepository<Branch> _branchRepository)
    : ICommandHandler<UpdateRoomTypeCommand, Result<RoomTypeDTO>>
{
    public async Task<Result<RoomTypeDTO>> Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        // Check if branch exists
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
        if (branch == null)
        {
            return Result.NotFound($"Branch with ID {request.BranchId} not found.");
        }

        // Get the room type to update
        var spec = new RoomTypeByIdWithBranchSpec(request.Id);
        var roomType = await _roomTypeRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (roomType == null)
        {
            return Result.NotFound($"RoomType with ID {request.Id} not found.");
        }

        // Update room type details
        roomType.UpdateDetails(
            request.BranchId,
            request.Name,
            request.BasePrice,
            request.Capacity,
            request.Description);

        await _roomTypeRepository.UpdateAsync(roomType, cancellationToken);
        await _roomTypeRepository.SaveChangesAsync(cancellationToken);

        var roomTypeDto = new RoomTypeDTO(
            roomType.Id,
            roomType.BranchId,
            branch.Name,
            roomType.Name,
            roomType.BasePrice,
            roomType.Capacity,
            roomType.Description);

        return Result.Success(roomTypeDto);
    }
}
