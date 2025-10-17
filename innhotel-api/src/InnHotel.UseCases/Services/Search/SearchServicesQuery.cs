namespace InnHotel.UseCases.Services.Search;

public record SearchServicesQuery(
    string? SearchTerm = null,
    int? BranchId = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<Result<IEnumerable<ServiceDTO>>>;