using InnHotel.Core.RoomAggregate;
using Ardalis.GuardClauses;

namespace InnHotel.Core.ReservationAggregate;

/// <summary>
/// Service for checking room availability and preventing double-booking.
/// </summary>
public class RoomAvailabilityService
{
    /// <summary>
    /// Checks if a room is available for the specified date range.
    /// </summary>
    /// <param name="roomId">The room to check</param>
    /// <param name="checkInDate">The desired check-in date</param>
    /// <param name="checkOutDate">The desired check-out date</param>
    /// <param name="existingReservations">All existing reservations</param>
    /// <param name="excludeReservationId">Optional reservation ID to exclude (for modifications)</param>
    /// <returns>True if the room is available, false otherwise</returns>
    public bool IsRoomAvailable(
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        IEnumerable<Reservation> existingReservations,
        int? excludeReservationId = null)
    {
        Guard.Against.NegativeOrZero(roomId, nameof(roomId));
        
        if (checkOutDate <= checkInDate)
        {
            throw new ArgumentException("Check-out date must be after check-in date.");
        }

        // Filter reservations for this room that are not cancelled
        var roomReservations = existingReservations
            .Where(r => r.Status != ReservationStatus.Cancelled && 
                       r.Status != ReservationStatus.CheckedOut)
            .Where(r => r.Rooms.Any(room => room.RoomId == roomId))
            .Where(r => !excludeReservationId.HasValue || r.Id != excludeReservationId.Value);

        // Check for date overlap
        foreach (var reservation in roomReservations)
        {
            if (DatesOverlap(checkInDate, checkOutDate, reservation.CheckInDate, reservation.CheckOutDate))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if multiple rooms are available for the specified date range.
    /// </summary>
    public Dictionary<int, bool> CheckBulkAvailability(
        IEnumerable<int> roomIds,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        IEnumerable<Reservation> existingReservations)
    {
        var result = new Dictionary<int, bool>();

        foreach (var roomId in roomIds)
        {
            result[roomId] = IsRoomAvailable(roomId, checkInDate, checkOutDate, existingReservations);
        }

        return result;
    }

    /// <summary>
    /// Gets all available rooms of a specific type for the date range.
    /// </summary>
    public IEnumerable<Room> GetAvailableRoomsByType(
        int roomTypeId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        IEnumerable<Room> allRooms,
        IEnumerable<Reservation> existingReservations)
    {
        var roomsOfType = allRooms.Where(r => r.RoomTypeId == roomTypeId && r.Status == RoomStatus.Available);

        return roomsOfType.Where(room => 
            IsRoomAvailable(room.Id, checkInDate, checkOutDate, existingReservations));
    }

    /// <summary>
    /// Gets all available rooms for a specific branch and date range.
    /// </summary>
    public IEnumerable<Room> GetAvailableRoomsByBranch(
        int branchId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        IEnumerable<Room> allRooms,
        IEnumerable<Reservation> existingReservations)
    {
        var branchRooms = allRooms.Where(r => r.BranchId == branchId && r.Status == RoomStatus.Available);

        return branchRooms.Where(room => 
            IsRoomAvailable(room.Id, checkInDate, checkOutDate, existingReservations));
    }

    /// <summary>
    /// Checks if two date ranges overlap.
    /// </summary>
    private bool DatesOverlap(DateOnly start1, DateOnly end1, DateOnly start2, DateOnly end2)
    {
        // Two date ranges overlap if:
        // - start1 is before end2 AND
        // - end1 is after start2
        return start1 < end2 && end1 > start2;
    }

    /// <summary>
    /// Validates that a reservation can be created without conflicts.
    /// </summary>
    public Result ValidateReservation(
        IEnumerable<int> roomIds,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        IEnumerable<Reservation> existingReservations)
    {
        var unavailableRooms = new List<int>();

        foreach (var roomId in roomIds)
        {
            if (!IsRoomAvailable(roomId, checkInDate, checkOutDate, existingReservations))
            {
                unavailableRooms.Add(roomId);
            }
        }

        if (unavailableRooms.Any())
        {
            return Result.Invalid(new ValidationError
            {
                Identifier = "RoomAvailability",
                ErrorMessage = $"The following rooms are not available for the selected dates: {string.Join(", ", unavailableRooms)}"
            });
        }

        return Result.Success();
    }
}