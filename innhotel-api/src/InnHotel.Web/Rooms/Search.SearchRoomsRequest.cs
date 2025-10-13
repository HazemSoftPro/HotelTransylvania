using InnHotel.Core.RoomAggregate;

namespace InnHotel.Web.Rooms;

/// <summary>
/// Request DTO to search Rooms
/// </summary>
public class SearchRoomsRequest
{
    public const string Route = "/rooms/search";

    public string? SearchTerm { get; set; }
    public int? BranchId { get; set; }
    public int? RoomTypeId { get; set; }
    public RoomStatus? Status { get; set; }
    public int? Floor { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
