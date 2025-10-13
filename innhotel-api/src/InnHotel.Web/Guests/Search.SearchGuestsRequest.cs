using InnHotel.Core.GuestAggregate;
using InnHotel.Core.GuestAggregate.ValueObjects;

namespace InnHotel.Web.Guests;

/// <summary>
/// Request DTO to search Guests
/// </summary>
public class SearchGuestsRequest
{
    public const string Route = "/guests/search";

    public string? SearchTerm { get; set; }
    public Gender? Gender { get; set; }
    public IdProofType? IdProofType { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
