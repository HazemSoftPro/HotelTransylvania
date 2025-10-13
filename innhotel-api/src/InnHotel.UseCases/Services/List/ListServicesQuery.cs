namespace InnHotel.UseCases.Services.List;

public record ListServicesQuery(int PageNumber = 1, int PageSize = 10) : IQuery<Result<IEnumerable<ServiceDTO>>>;
