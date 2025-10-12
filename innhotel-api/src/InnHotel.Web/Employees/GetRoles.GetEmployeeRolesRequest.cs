namespace InnHotel.Web.Employees;

/// <summary>
/// Request DTO to get Employee roles
/// </summary>
public class GetEmployeeRolesRequest
{
    public const string Route = "/employees/{id}/roles";

    public int Id { get; set; }
}
