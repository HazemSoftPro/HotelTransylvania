namespace InnHotel.Web.Employees;

/// <summary>
/// Request DTO to assign roles to an Employee
/// </summary>
public class AssignEmployeeRolesRequest
{
    public const string Route = "/employees/{id}/roles";

    public int Id { get; set; }
    public List<string> Roles { get; set; } = new();
}
