using System.ComponentModel.DataAnnotations;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Request DTO to create a new RoomType
/// </summary>
public class CreateRoomTypeRequest
{
    public const string Route = "/roomtypes";

    [Required]
    public int BranchId { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Base price must be greater than 0")]
    public decimal BasePrice { get; set; }

    [Required]
    [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20")]
    public int Capacity { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}
