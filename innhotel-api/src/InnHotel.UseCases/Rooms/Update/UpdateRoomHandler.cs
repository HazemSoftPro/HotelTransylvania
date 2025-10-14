using InnHotel.Core.RoomAggregate;
using InnHotel.Core.RoomAggregate.Specifications;
using InnHotel.Core.BranchAggregate;
using InnHotel.Core.BranchAggregate.Specifications;

namespace InnHotel.UseCases.Rooms.Update;

public class UpdateRoomHandler(
    IRepository<Room> _roomRepository,
    IReadRepository<Branch> _branchRepository,
    IReadRepository<RoomType> _roomTypeRepository)
    : ICommandHandler<UpdateRoomCommand, Result<RoomDTO>>
{
    public async Task<Result<RoomDTO>> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {


        // Get the existing room
        var room = await _roomRepository.GetByIdAsync(request.RoomId, cancellationToken);
        if (room == null)
            return Result.NotFound("Room not found");

        // Validate room type exists
        var roomType = await _roomTypeRepository.GetByIdAsync(request.RoomTypeId, cancellationToken);
        if (roomType == null)
            return Result.NotFound("Room type not found");

        // Check if room number is unique in the branch (excluding current room)
        var spec = new RoomByBranchAndNumberSpec(room.BranchId, request.RoomNumber);
        var existingRoom = await _roomRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (existingRoom != null && existingRoom.Id != request.RoomId)
            return Result.Error("A room with this number already exists in the branch");

        // Update room details
        room.UpdateDetails(
            request.RoomTypeId,
            request.RoomNumber,
            request.Status,
            request.Floor,
            request.ManualPrice);

        await _roomRepository.UpdateAsync(room, cancellationToken);



        // Get branch for the response
        var branch = await _branchRepository.GetByIdAsync(room.BranchId, cancellationToken);
        if (branch == null)
            return Result.Error("Branch not found");

        return new RoomDTO(
            room.Id,
            room.BranchId,
            branch.Name,
            room.RoomTypeId,
            roomType.Name,
            roomType.Capacity,
            room.RoomNumber,
            room.Status,
            room.Floor,
            room.ManualPrice);
    }
}
