namespace InnHotel.Web.Guests;

/// <summary>
/// Request DTO to get Guest History
/// </summary>
public class GetGuestHistoryRequest
{
    public const string Route = "/guests/{id}/history";

    public int Id { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
