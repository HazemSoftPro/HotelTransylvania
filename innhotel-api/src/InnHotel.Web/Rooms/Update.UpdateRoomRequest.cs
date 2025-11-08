using System.ComponentModel.DataAnnotations;

namespace InnHotel.Web.Rooms;

public class UpdateRoomRequest
{
    public const string Route = "/Rooms/{RoomId:int}";
    public static string BuildRoute(int roomId) => Route.Replace("{RoomId:int}", roomId.ToString());

    public int RoomId { get; set; }
    
    [Required]
    public int RoomTypeId { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string RoomNumber { get; set; } = string.Empty;
    
    [Required]
    public RoomStatus Status { get; set; }
    
    [Required]
    [Range(0, 100)]
    public int Floor { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal ManualPrice { get; set; }
}
