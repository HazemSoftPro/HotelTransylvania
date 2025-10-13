using InnHotel.Core.RoomAggregate;

namespace InnHotel.UseCases.RoomTypes.Delete;

public class DeleteRoomTypeHandler(IRepository<RoomType> _repository)
    : ICommandHandler<DeleteRoomTypeCommand, Result>
{
    public async Task<Result> Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var roomType = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (roomType == null)
        {
            return Result.NotFound($"RoomType with ID {request.Id} not found.");
        }

        await _repository.DeleteAsync(roomType, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
