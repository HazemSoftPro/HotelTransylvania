using System.ComponentModel.DataAnnotations;

namespace InnHotel.Web.Services;

/// <summary>
/// Request DTO to get a Service by ID
/// </summary>
public class GetServiceByIdRequest
{
    public const string Route = "/services/{id}";

    [Required]
    public int Id { get; set; }
}
