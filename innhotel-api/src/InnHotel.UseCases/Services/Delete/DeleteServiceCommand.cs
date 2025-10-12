namespace InnHotel.UseCases.Services.Delete;

public record DeleteServiceCommand(int Id) : ICommand<Result>;
