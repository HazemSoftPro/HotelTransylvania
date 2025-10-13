namespace InnHotel.Web.Services;

/// <summary>
/// Service record for API responses
/// </summary>
public record ServiceRecord(
    int Id,
    int BranchId,
    string BranchName,
    string Name,
    decimal Price,
    string? Description);
