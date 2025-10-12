namespace InnHotel.Web.Services;

/// <summary>
/// Request DTO to list Services
/// </summary>
public class ListServiceRequest
{
    public const string Route = "/services";

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
