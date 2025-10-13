using System.ComponentModel.DataAnnotations;

namespace InnHotel.Web.Services;

/// <summary>
/// Request DTO to delete a Service
/// </summary>
public class DeleteServiceRequest
{
    public const string Route = "/services/{id}";

    [Required]
    public int Id { get; set; }
}
