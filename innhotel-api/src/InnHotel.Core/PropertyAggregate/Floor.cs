using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.PropertyAggregate;

/// <summary>
/// Represents a floor in the hotel
/// </summary>
public class Floor : AuditableEntity
{
    public int HotelId { get; private set; }
    public int FloorNumber { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public Hotel Hotel { get; private set; } = null!;
    public ICollection<Room> Rooms { get; private set; } = new List<Room>();

    private Floor() { } // EF Core constructor

    public Floor(int hotelId, int floorNumber, string name, string? description = null)
    {
        HotelId = Guard.Against.NegativeOrZero(hotelId, nameof(hotelId));
        FloorNumber = floorNumber;
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Description = description;
        IsActive = true;
    }

    public void UpdateDetails(int floorNumber, string name, string? description = null)
    {
        FloorNumber = floorNumber;
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Description = description;
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
