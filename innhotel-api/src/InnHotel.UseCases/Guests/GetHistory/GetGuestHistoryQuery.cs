namespace InnHotel.UseCases.Guests.GetHistory;

public record GetGuestHistoryQuery(
    int GuestId,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<Result<IEnumerable<ReservationDTO>>>;
