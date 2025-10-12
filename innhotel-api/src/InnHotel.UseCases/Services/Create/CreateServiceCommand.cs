namespace InnHotel.UseCases.Services.Create;

public record CreateServiceCommand(
    int BranchId,
    string Name,
    decimal Price,
    string? Description) : ICommand<Result<ServiceDTO>>;
