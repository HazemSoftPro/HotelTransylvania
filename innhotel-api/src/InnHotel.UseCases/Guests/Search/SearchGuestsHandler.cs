using InnHotel.Core.GuestAggregate;
using InnHotel.Core.GuestAggregate.Specifications;

namespace InnHotel.UseCases.Guests.Search;

public class SearchGuestsHandler(IReadRepository<Guest> _repository)
    : IQueryHandler<SearchGuestsQuery, Result<IEnumerable<GuestDTO>>>
{
    public async Task<Result<IEnumerable<GuestDTO>>> Handle(SearchGuestsQuery request, CancellationToken cancellationToken)
    {
        var spec = new GuestSearchSpec(
            request.SearchTerm,
            request.Gender,
            request.IdProofType,
            request.PageNumber,
            request.PageSize);

        var guests = await _repository.ListAsync(spec, cancellationToken);

        var guestDtos = guests.Select(g => new GuestDTO(
            g.Id,
            g.FirstName,
            g.LastName,
            g.Gender,
            g.IdProofType,
            g.IdProofNumber,
            g.Email,
            g.Phone,
            g.Address)).ToList();

        return Result.Success(guestDtos.AsEnumerable());
    }
}
