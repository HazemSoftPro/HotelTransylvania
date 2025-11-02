namespace InnHotel.Web.Reservations;

public class SearchReservationsRequest
{
    public const string Route = "/reservations/search";
    
    public string? SearchTerm { get; set; }
    public int? GuestId { get; set; }
    public ReservationStatus? Status { get; set; }
    public DateTime? CheckInDateFrom { get; set; }
    public DateTime? CheckInDateTo { get; set; }
    public DateTime? CheckOutDateFrom { get; set; }
    public DateTime? CheckOutDateTo { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}