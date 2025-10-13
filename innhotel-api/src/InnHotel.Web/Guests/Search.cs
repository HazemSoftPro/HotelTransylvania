using InnHotel.UseCases.Guests.Search;
using InnHotel.Web.Common;
using AuthRoles = InnHotel.Core.AuthAggregate.Roles;

namespace InnHotel.Web.Guests;

/// <summary>
/// Search Guests with filters.
/// </summary>
/// <remarks>
/// Searches and filters guests based on various criteria including search term, gender, and ID proof type.
/// </remarks>
public class Search(IMediator _mediator)
    : Endpoint<SearchGuestsRequest, object>
{
    public override void Configure()
    {
        Get(SearchGuestsRequest.Route);
        Roles(AuthRoles.SuperAdmin, AuthRoles.Admin, AuthRoles.Employee);
        Summary(s =>
        {
            s.ExampleRequest = new SearchGuestsRequest
            {
                SearchTerm = "john",
                Gender = Core.GuestAggregate.Gender.Male,
                IdProofType = Core.GuestAggregate.IdProofType.Passport,
                PageNumber = 1,
                PageSize = 10
            };
        });
    }

    public override async Task HandleAsync(
        SearchGuestsRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchGuestsQuery(
            request.SearchTerm,
            request.Gender,
            request.IdProofType,
            request.PageNumber,
            request.PageSize);

        var result = await _mediator.Send(query, cancellationToken);

        if (result.Status == ResultStatus.Error)
        {
            await SendAsync(
                new FailureResponse(400, result.Errors.First()),
                statusCode: 400,
                cancellation: cancellationToken);
            return;
        }

        if (result.IsSuccess)
        {
            var guestRecords = result.Value.Select(g => new GuestRecord(
                g.Id,
                g.FirstName,
                g.LastName,
                g.Gender,
                g.IdProofType,
                g.IdProofNumber,
                g.Email,
                g.Phone,
                g.Address)).ToList();

            await SendAsync(
                new { 
                    status = 200, 
                    message = "Guests search completed successfully", 
                    data = guestRecords,
                    pagination = new {
                        pageNumber = request.PageNumber,
                        pageSize = request.PageSize,
                        totalResults = guestRecords.Count
                    }
                },
                statusCode: 200,
                cancellation: cancellationToken);
            return;
        }

        await SendAsync(
            new FailureResponse(500, "An unexpected error occurred."),
            statusCode: 500,
            cancellation: cancellationToken);
    }
}
