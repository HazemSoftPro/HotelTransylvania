using System.ComponentModel.DataAnnotations;

namespace InnHotel.Web.Reservations;

/// <summary>
/// Request DTO to create a new Reservation
/// </summary>
public class CreateReservationRequest
{
    public const string Route = "/reservations";

    [Required]
    public int GuestId { get; set; }

    [Required]
    public DateOnly CheckInDate { get; set; }

    [Required]
    public DateOnly CheckOutDate { get; set; }

    [Required]
    [MinLength(1)]
    public List<ReservationRoomRequest> Rooms { get; set; } = new();

    public List<ReservationServiceRequest>? Services { get; set; }
}

/// <summary>
/// Room details for reservation
/// </summary>
public class ReservationRoomRequest
{
    [Required]
    public int RoomId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal PricePerNight { get; set; }
}

/// <summary>
/// Service details for reservation
/// </summary>
public class ReservationServiceRequest
{
    [Required]
    public int ServiceId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}
