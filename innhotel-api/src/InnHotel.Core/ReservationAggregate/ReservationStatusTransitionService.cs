using Ardalis.GuardClauses;

namespace InnHotel.Core.ReservationAggregate;

/// <summary>
/// Service for managing reservation status transitions with validation rules.
/// Ensures that status changes follow proper business logic and prevent invalid transitions.
/// </summary>
public class ReservationStatusTransitionService
{
    /// <summary>
    /// Validates if a status transition is allowed based on business rules.
    /// </summary>
    /// <param name="currentStatus">The current reservation status</param>
    /// <param name="newStatus">The desired new status</param>
    /// <returns>True if the transition is valid, false otherwise</returns>
    public bool IsValidTransition(ReservationStatus currentStatus, ReservationStatus newStatus)
    {
        // Same status is always valid (no-op)
        if (currentStatus == newStatus)
            return true;

        return currentStatus switch
        {
            // From Pending: can go to Confirmed or Cancelled
            ReservationStatus.Pending => newStatus is ReservationStatus.Confirmed or ReservationStatus.Cancelled,
            
            // From Confirmed: can go to CheckedIn or Cancelled
            ReservationStatus.Confirmed => newStatus is ReservationStatus.CheckedIn or ReservationStatus.Cancelled,
            
            // From CheckedIn: can only go to CheckedOut
            ReservationStatus.CheckedIn => newStatus == ReservationStatus.CheckedOut,
            
            // From CheckedOut: no further transitions allowed (terminal state)
            ReservationStatus.CheckedOut => false,
            
            // From Cancelled: no further transitions allowed (terminal state)
            ReservationStatus.Cancelled => false,
            
            _ => false
        };
    }

    /// <summary>
    /// Validates and transitions a reservation to a new status.
    /// </summary>
    /// <param name="reservation">The reservation to transition</param>
    /// <param name="newStatus">The desired new status</param>
    /// <exception cref="InvalidOperationException">Thrown when the transition is not allowed</exception>
    public void TransitionStatus(Reservation reservation, ReservationStatus newStatus)
    {
        Guard.Against.Null(reservation, nameof(reservation));

        if (!IsValidTransition(reservation.Status, newStatus))
        {
            throw new InvalidOperationException(
                $"Cannot transition reservation from {reservation.Status} to {newStatus}. " +
                $"Valid transitions from {reservation.Status} are: {GetValidTransitions(reservation.Status)}");
        }

        // Additional business rule validations
        ValidateBusinessRules(reservation, newStatus);

        reservation.Status = newStatus;
    }

    /// <summary>
    /// Gets a comma-separated list of valid status transitions from the current status.
    /// </summary>
    private string GetValidTransitions(ReservationStatus currentStatus)
    {
        var validStatuses = Enum.GetValues<ReservationStatus>()
            .Where(status => IsValidTransition(currentStatus, status) && status != currentStatus)
            .Select(s => s.ToString());

        return string.Join(", ", validStatuses);
    }

    /// <summary>
    /// Validates additional business rules for status transitions.
    /// </summary>
    private void ValidateBusinessRules(Reservation reservation, ReservationStatus newStatus)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        switch (newStatus)
        {
            case ReservationStatus.CheckedIn:
                // Can only check in on or after the check-in date
                if (today < reservation.CheckInDate)
                {
                    throw new InvalidOperationException(
                        $"Cannot check in before the reservation check-in date ({reservation.CheckInDate}).");
                }
                // Cannot check in after check-out date
                if (today > reservation.CheckOutDate)
                {
                    throw new InvalidOperationException(
                        $"Cannot check in after the reservation check-out date ({reservation.CheckOutDate}).");
                }
                break;

            case ReservationStatus.CheckedOut:
                // Can only check out on or after check-in date
                if (today < reservation.CheckInDate)
                {
                    throw new InvalidOperationException(
                        "Cannot check out before the check-in date.");
                }
                break;

            case ReservationStatus.Confirmed:
                // Cannot confirm a reservation that has already passed
                if (today > reservation.CheckOutDate)
                {
                    throw new InvalidOperationException(
                        "Cannot confirm a reservation that has already passed.");
                }
                break;
        }
    }

    /// <summary>
    /// Gets all possible status transitions from the current status.
    /// </summary>
    public IEnumerable<ReservationStatus> GetPossibleTransitions(ReservationStatus currentStatus)
    {
        return Enum.GetValues<ReservationStatus>()
            .Where(status => IsValidTransition(currentStatus, status) && status != currentStatus);
    }
}