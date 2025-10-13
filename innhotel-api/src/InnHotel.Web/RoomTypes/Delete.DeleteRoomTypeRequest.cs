using System.ComponentModel.DataAnnotations;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Request DTO to delete a RoomType
/// </summary>
public class DeleteRoomTypeRequest
{
    public const string Route = "/roomtypes/{id}";

    [Required]
    public int Id { get; set; }
}
