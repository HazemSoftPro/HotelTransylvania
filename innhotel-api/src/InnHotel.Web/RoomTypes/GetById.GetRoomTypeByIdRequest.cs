using System.ComponentModel.DataAnnotations;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Request DTO to get a RoomType by ID
/// </summary>
public class GetRoomTypeByIdRequest
{
    public const string Route = "/roomtypes/{id}";

    [Required]
    public int Id { get; set; }
}
