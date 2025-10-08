namespace InnHotel.Web.Reservations;

public class GetReservationByIdRequest
{
    public const string Route = "/reservations/{Id}";
    public int Id { get; set; }
}
