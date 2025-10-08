using InnHotel.Web.Common;

namespace InnHotel.Web.Reservations;

public class ListReservationsRequest : PaginationRequest
{
    public const string Route = "/reservations";
}
