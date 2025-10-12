namespace InnHotel.UseCases.Services;

/// <summary>
/// Data Transfer Object for Service
/// </summary>
public record ServiceDTO(
    int Id,
    int BranchId,
    string BranchName,
    string Name,
    decimal Price,
    string? Description);
