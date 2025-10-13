using InnHotel.Core.GuestAggregate;

namespace InnHotel.UseCases.Guests.Search;

public record SearchGuestsQuery(
    string? SearchTerm = null,
    Gender? Gender = null,
    IdProofType? IdProofType = null,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<Result<IEnumerable<GuestDTO>>>;
