namespace InnHotel.UseCases.Services.Update;

public record UpdateServiceCommand(
    int Id,
    int BranchId,
    string Name,
    decimal Price,
    string? Description) : ICommand<Result<ServiceDTO>>;
