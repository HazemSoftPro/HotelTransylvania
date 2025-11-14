using InnHotel.Core.ReservationAggregate;

namespace InnHotel.Core.Interfaces;

/// <summary>
/// Service for managing reservations
/// </summary>
public interface IReservationService
{
    Task<IEnumerable<Reservation>> GetWaitlistedReservationsForRoomTypeAsync(int roomId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Reservation>> GetUpcomingReservationsForRoomAsync(int roomId, CancellationToken cancellationToken = default);
    Task<bool> IsRoomAvailableAsync(int roomId, DateOnly checkInDate, DateOnly checkOutDate, CancellationToken cancellationToken = default);
    Task<Reservation?> GetReservationByIdAsync(int reservationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Reservation>> GetReservationsByGuestIdAsync(int guestId, CancellationToken cancellationToken = default);
}