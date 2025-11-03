namespace InnHotel.Web.RoomTypes;

public class SearchRoomTypesRequest
{
    public const string Route = "/roomtypes/search";
    
    public string? SearchTerm { get; set; }
    public int? BranchId { get; set; }
    public int? MinCapacity { get; set; }
    public int? MaxCapacity { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}