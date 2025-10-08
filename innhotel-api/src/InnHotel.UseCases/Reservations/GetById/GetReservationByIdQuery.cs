using MediatR;
using Ardalis.Result;

namespace InnHotel.UseCases.Reservations.GetById;

public record GetReservationByIdQuery(int Id) : IRequest<Result<ReservationDto>>;
