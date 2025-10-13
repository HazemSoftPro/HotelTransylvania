using InnHotel.Core.ReservationAggregate;

namespace InnHotel.UseCases.Reservations.Search;

public record SearchReservationsQuery(
    string? SearchTerm = null,
    int? GuestId = null,
    ReservationStatus? Status = null,
    DateTime? CheckInDateFrom = null,
    DateTime? CheckInDateTo = null,
    DateTime? CheckOutDateFrom = null,
    DateTime? CheckOutDateTo = null,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<Result<IEnumerable<ReservationDto>>>;
