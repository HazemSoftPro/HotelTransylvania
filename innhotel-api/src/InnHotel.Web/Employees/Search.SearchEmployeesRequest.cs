namespace InnHotel.Web.Employees;

public class SearchEmployeesRequest
{
    public const string Route = "/employees/search";
    
    public string? SearchTerm { get; set; }
    public int? BranchId { get; set; }
    public string? Position { get; set; }
    public DateTime? HireDateFrom { get; set; }
    public DateTime? HireDateTo { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}