using InnHotel.Core.RoomAggregate;
using Ardalis.GuardClauses;

namespace InnHotel.Core.ReservationAggregate;

/// <summary>
/// Service for synchronizing room status with reservation status changes.
/// Automatically updates room availability based on check-in and check-out events.
/// </summary>
public class RoomStatusSyncService
{
    /// <summary>
    /// Synchronizes room status based on reservation status change.
    /// </summary>
    /// <param name="reservation">The reservation that changed status</param>
    /// <param name="rooms">The rooms associated with the reservation</param>
    /// <param name="newReservationStatus">The new reservation status</param>
    public void SyncRoomStatus(
        Reservation reservation, 
        IEnumerable<Room> rooms, 
        ReservationStatus newReservationStatus)
    {
        Guard.Against.Null(reservation, nameof(reservation));
        Guard.Against.Null(rooms, nameof(rooms));

        var roomsList = rooms.ToList();
        if (!roomsList.Any())
            return;

        switch (newReservationStatus)
        {
            case ReservationStatus.CheckedIn:
                // Mark all rooms as Occupied when guest checks in
                foreach (var room in roomsList)
                {
                    if (room.Status != RoomStatus.Occupied)
                    {
                        room.UpdateStatus(RoomStatus.Occupied);
                    }
                }
                break;

            case ReservationStatus.CheckedOut:
                // Mark all rooms as Available when guest checks out
                // Note: In a real system, you might want to mark as "NeedsCleaning" first
                foreach (var room in roomsList)
                {
                    if (room.Status == RoomStatus.Occupied)
                    {
                        room.UpdateStatus(RoomStatus.Available);
                    }
                }
                break;

            case ReservationStatus.Cancelled:
                // If reservation is cancelled before check-in, ensure rooms are available
                if (reservation.Status == ReservationStatus.Pending || 
                    reservation.Status == ReservationStatus.Confirmed)
                {
                    foreach (var room in roomsList)
                    {
                        if (room.Status == RoomStatus.Occupied)
                        {
                            room.UpdateStatus(RoomStatus.Available);
                        }
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Determines the appropriate room status based on reservation status.
    /// </summary>
    public RoomStatus GetTargetRoomStatus(ReservationStatus reservationStatus)
    {
        return reservationStatus switch
        {
            ReservationStatus.CheckedIn => RoomStatus.Occupied,
            ReservationStatus.CheckedOut => RoomStatus.Available,
            ReservationStatus.Cancelled => RoomStatus.Available,
            _ => RoomStatus.Available // Default for Pending and Confirmed
        };
    }
}