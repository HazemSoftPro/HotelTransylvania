using InnHotel.Core.BranchAggregate;
using InnHotel.Core.RoomAggregate;

namespace InnHotel.UseCases.RoomTypes.Create;

public class CreateRoomTypeHandler(IRepository<RoomType> _roomTypeRepository, IRepository<Branch> _branchRepository)
    : ICommandHandler<CreateRoomTypeCommand, Result<RoomTypeDTO>>
{
    public async Task<Result<RoomTypeDTO>> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        // Check if branch exists
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
        if (branch == null)
        {
            return Result.NotFound($"Branch with ID {request.BranchId} not found.");
        }

        // Create new room type
        var roomType = new RoomType(
            request.BranchId,
            request.Name,
            request.Capacity,
            request.Description);

        var createdRoomType = await _roomTypeRepository.AddAsync(roomType, cancellationToken);
        await _roomTypeRepository.SaveChangesAsync(cancellationToken);

        var roomTypeDto = new RoomTypeDTO(
            createdRoomType.Id,
            createdRoomType.BranchId,
            branch.Name,
            createdRoomType.Name,
            createdRoomType.Capacity,
            createdRoomType.Description);

        return Result.Success(roomTypeDto);
    }
}
