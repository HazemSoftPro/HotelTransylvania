namespace InnHotel.Web.Services;

public class SearchServicesRequest
{
    public const string Route = "/services/search";
    
    public string? SearchTerm { get; set; }
    public int? BranchId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}