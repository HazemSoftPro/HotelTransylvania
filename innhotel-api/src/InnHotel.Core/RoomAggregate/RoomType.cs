using InnHotel.Core.BranchAggregate;
using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.RoomAggregate;

public class RoomType : AuditableEntity, IAggregateRoot
{
    public int BranchId { get; private set; }
    public string Name { get; private set; }
    public decimal BasePrice { get; private set; }
    public int MaxOccupancy { get; private set; }
    public int MaxAdults { get; private set; }
    public int MaxChildren { get; private set; }
    public string? Description { get; private set; }
    public decimal? Size { get; private set; } // in square meters
    public string? BedConfiguration { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public Branch Branch { get; private set; } = null!;
    public ICollection<Room> Rooms { get; private set; } = new List<Room>();
    public ICollection<RoomTypeAmenity> Amenities { get; private set; } = new List<RoomTypeAmenity>();
    public ICollection<RoomTypeImage> Images { get; private set; } = new List<RoomTypeImage>();
    public ICollection<RatePlan> RatePlans { get; private set; } = new List<RatePlan>();

    private RoomType() { } // EF Core constructor

    public RoomType(
        int branchId,
        string name,
        decimal basePrice,
        int maxOccupancy,
        int maxAdults,
        int maxChildren,
        string? description = null,
        decimal? size = null,
        string? bedConfiguration = null
    )
    {
        BranchId = Guard.Against.NegativeOrZero(branchId, nameof(branchId));
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        BasePrice = Guard.Against.NegativeOrZero(basePrice, nameof(basePrice));
        MaxOccupancy = Guard.Against.NegativeOrZero(maxOccupancy, nameof(maxOccupancy));
        MaxAdults = Guard.Against.NegativeOrZero(maxAdults, nameof(maxAdults));
        MaxChildren = Guard.Against.Negative(maxChildren, nameof(maxChildren));
        Description = description;
        Size = size;
        BedConfiguration = bedConfiguration;
        IsActive = true;
    }

    public void UpdateDetails(
        string name,
        decimal basePrice,
        int maxOccupancy,
        int maxAdults,
        int maxChildren,
        string? description = null,
        decimal? size = null,
        string? bedConfiguration = null
    )
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        BasePrice = Guard.Against.NegativeOrZero(basePrice, nameof(basePrice));
        MaxOccupancy = Guard.Against.NegativeOrZero(maxOccupancy, nameof(maxOccupancy));
        MaxAdults = Guard.Against.NegativeOrZero(maxAdults, nameof(maxAdults));
        MaxChildren = Guard.Against.Negative(maxChildren, nameof(maxChildren));
        Description = description;
        Size = size;
        BedConfiguration = bedConfiguration;
        UpdateAuditInfo();
    }

    public void AddAmenity(string name, string? description = null)
    {
        var amenity = new RoomTypeAmenity(Id, name, description);
        Amenities.Add(amenity);
        UpdateAuditInfo();
    }

    public void RemoveAmenity(int amenityId)
    {
        var amenity = Amenities.FirstOrDefault(a => a.Id == amenityId);
        if (amenity != null)
        {
            Amenities.Remove(amenity);
            UpdateAuditInfo();
        }
    }

    public void AddImage(string imageUrl, string? caption = null, bool isPrimary = false)
    {
        if (isPrimary)
        {
            // Set all existing images as non-primary
            foreach (var existingImage in Images)
            {
                existingImage.SetAsSecondary();
            }
        }

        var image = new RoomTypeImage(Id, imageUrl, caption, isPrimary);
        Images.Add(image);
        UpdateAuditInfo();
    }

    public void RemoveImage(int imageId)
    {
        var image = Images.FirstOrDefault(i => i.Id == imageId);
        if (image != null)
        {
            Images.Remove(image);
            UpdateAuditInfo();
        }
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
