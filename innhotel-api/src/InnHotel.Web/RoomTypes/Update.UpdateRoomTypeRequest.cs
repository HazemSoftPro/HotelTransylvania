using System.ComponentModel.DataAnnotations;

namespace InnHotel.Web.RoomTypes;

/// <summary>
/// Request DTO to update a RoomType
/// </summary>
public class UpdateRoomTypeRequest
{
    public const string Route = "/roomtypes/{id}";

    [Required]
    public int Id { get; set; }

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
