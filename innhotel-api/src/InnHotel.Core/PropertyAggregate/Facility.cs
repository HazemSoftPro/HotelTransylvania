using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.PropertyAggregate;

/// <summary>
/// Represents hotel facilities (Pool, Gym, Spa, Conference Hall, etc.)
/// </summary>
public class Facility : AuditableEntity
{
    public int HotelId { get; private set; }
    public string Name { get; private set; }
    public string Type { get; private set; }
    public string? Description { get; private set; }
    public string? Location { get; private set; }
    public int? Capacity { get; private set; }
    public TimeSpan? OpeningTime { get; private set; }
    public TimeSpan? ClosingTime { get; private set; }
    public bool IsBookable { get; private set; }
    public decimal? HourlyRate { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public Hotel Hotel { get; private set; } = null!;
    public ICollection<FacilityBooking> Bookings { get; private set; } = new List<FacilityBooking>();

    private Facility() { } // EF Core constructor

    public Facility(
        int hotelId,
        string name,
        string type,
        string? description = null,
        string? location = null,
        int? capacity = null,
        bool isBookable = false
    )
    {
        HotelId = Guard.Against.NegativeOrZero(hotelId, nameof(hotelId));
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Type = Guard.Against.NullOrEmpty(type, nameof(type));
        Description = description;
        Location = location;
        Capacity = capacity;
        IsBookable = isBookable;
        IsActive = true;
    }

    public void UpdateDetails(
        string name,
        string type,
        string? description = null,
        string? location = null,
        int? capacity = null
    )
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Type = Guard.Against.NullOrEmpty(type, nameof(type));
        Description = description;
        Location = location;
        Capacity = capacity;
        UpdateAuditInfo();
    }

    public void UpdateSchedule(TimeSpan? openingTime, TimeSpan? closingTime)
    {
        OpeningTime = openingTime;
        ClosingTime = closingTime;
        UpdateAuditInfo();
    }

    public void UpdateBookingSettings(bool isBookable, decimal? hourlyRate = null)
    {
        IsBookable = isBookable;
        HourlyRate = isBookable ? hourlyRate : null;
        UpdateAuditInfo();
    }

    public void Activate()
    {
        IsActive = true;
        UpdateAuditInfo();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateAuditInfo();
    }
}

public static class FacilityTypes
{
    public const string Pool = "Pool";
    public const string Gym = "Gym";
    public const string Spa = "Spa";
    public const string ConferenceHall = "ConferenceHall";
    public const string Restaurant = "Restaurant";
    public const string Bar = "Bar";
    public const string BusinessCenter = "BusinessCenter";
    public const string Parking = "Parking";
    public const string Laundry = "Laundry";
}
