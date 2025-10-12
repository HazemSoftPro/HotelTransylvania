namespace InnHotel.UseCases.Services.Get;

public record GetServiceByIdQuery(int Id) : IQuery<Result<ServiceDTO>>;
