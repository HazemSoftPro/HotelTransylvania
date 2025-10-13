using System.ComponentModel.DataAnnotations;

namespace InnHotel.Web.Services;

/// <summary>
/// Request DTO to update a Service
/// </summary>
public class UpdateServiceRequest
{
    public const string Route = "/services/{id}";

    [Required]
    public int Id { get; set; }

    [Required]
    public int BranchId { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
    public decimal Price { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}
