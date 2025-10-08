using MediatR;
using Ardalis.Result;

namespace InnHotel.UseCases.Reservations.List;

public record ListReservationsQuery(int PageNumber, int PageSize) : IRequest<Result<(List<ReservationDto> Items, int TotalCount)>>;
