using InnHotel.Core.ReservationAggregate;
using InnHotel.Core.ReservationAggregate.Specifications;
using InnHotel.Core.RoomAggregate;
using InnHotel.Core.RoomAggregate.Specifications;

namespace InnHotel.UseCases.Reservations.UpdateStatus;

public class UpdateReservationStatusHandler(
    IRepository<Reservation> reservationRepository,
    IRepository<Room> roomRepository,
    ReservationStatusTransitionService statusTransitionService,
    RoomStatusSyncService roomStatusSyncService)
    : ICommandHandler<UpdateReservationStatusCommand, Result>
{
    public async Task<Result> Handle(UpdateReservationStatusCommand request, CancellationToken cancellationToken)
    {
        // Parse the status string to enum
        if (!Enum.TryParse<ReservationStatus>(request.NewStatus, true, out var newStatus))
        {
            return Result.Invalid(new ValidationError
            {
                Identifier = nameof(request.NewStatus),
                ErrorMessage = $"Invalid reservation status: {request.NewStatus}"
            });
        }

        // Get the reservation with rooms
        var spec = new ReservationByIdSpec(request.ReservationId);
        var reservation = await reservationRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (reservation == null)
        {
            return Result.NotFound($"Reservation with ID {request.ReservationId} not found.");
        }

        // Validate and perform status transition
        try
        {
            statusTransitionService.TransitionStatus(reservation, newStatus);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Invalid(new ValidationError
            {
                Identifier = nameof(request.NewStatus),
                ErrorMessage = ex.Message
            });
        }

        // Get the rooms associated with this reservation
        var roomIds = reservation.Rooms.Select(r => r.RoomId).ToList();
        var rooms = await roomRepository.ListAsync(cancellationToken);
        var reservationRooms = rooms.Where(r => roomIds.Contains(r.Id)).ToList();

        // Sync room status based on reservation status
        roomStatusSyncService.SyncRoomStatus(reservation, reservationRooms, newStatus);

        // Save changes
        await reservationRepository.UpdateAsync(reservation, cancellationToken);
        
        // Update rooms if their status changed
        foreach (var room in reservationRooms)
        {
            await roomRepository.UpdateAsync(room, cancellationToken);
        }

        return Result.Success();
    }
}