using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.PropertyAggregate;

/// <summary>
/// Represents the hotel property with all its configuration and policies
/// </summary>
public class Hotel : AuditableEntity, IAggregateRoot
{
    public string LegalName { get; private set; }
    public string TradingName { get; private set; }
    public string CommercialRegistration { get; private set; }
    public string Address { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
    public string? PostalCode { get; private set; }
    public string Phone { get; private set; }
    public string? Fax { get; private set; }
    public string Email { get; private set; }
    public string? Website { get; private set; }
    public string? LogoUrl { get; private set; }
    
    // Hotel Policies
    public TimeSpan CheckInTime { get; private set; }
    public TimeSpan CheckOutTime { get; private set; }
    public string? ChildPolicy { get; private set; }
    public string? PetPolicy { get; private set; }
    public string? CancellationPolicy { get; private set; }
    
    // Configuration
    public string Currency { get; private set; }
    public string TimeZone { get; private set; }
    public string Language { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public ICollection<Floor> Floors { get; private set; } = new List<Floor>();
    public ICollection<Facility> Facilities { get; private set; } = new List<Facility>();

    private Hotel() { } // EF Core constructor

    public Hotel(
        string legalName,
        string tradingName,
        string commercialRegistration,
        string address,
        string city,
        string country,
        string phone,
        string email,
        TimeSpan checkInTime,
        TimeSpan checkOutTime,
        string currency = "USD",
        string timeZone = "UTC",
        string language = "en"
    )
    {
        LegalName = Guard.Against.NullOrEmpty(legalName, nameof(legalName));
        TradingName = Guard.Against.NullOrEmpty(tradingName, nameof(tradingName));
        CommercialRegistration = Guard.Against.NullOrEmpty(commercialRegistration, nameof(commercialRegistration));
        Address = Guard.Against.NullOrEmpty(address, nameof(address));
        City = Guard.Against.NullOrEmpty(city, nameof(city));
        Country = Guard.Against.NullOrEmpty(country, nameof(country));
        Phone = Guard.Against.NullOrEmpty(phone, nameof(phone));
        Email = Guard.Against.NullOrEmpty(email, nameof(email));
        CheckInTime = checkInTime;
        CheckOutTime = checkOutTime;
        Currency = currency;
        TimeZone = timeZone;
        Language = language;
        IsActive = true;
    }

    public void UpdateBasicInfo(
        string legalName,
        string tradingName,
        string commercialRegistration,
        string address,
        string city,
        string country,
        string phone,
        string email,
        string? postalCode = null,
        string? fax = null,
        string? website = null
    )
    {
        LegalName = Guard.Against.NullOrEmpty(legalName, nameof(legalName));
        TradingName = Guard.Against.NullOrEmpty(tradingName, nameof(tradingName));
        CommercialRegistration = Guard.Against.NullOrEmpty(commercialRegistration, nameof(commercialRegistration));
        Address = Guard.Against.NullOrEmpty(address, nameof(address));
        City = Guard.Against.NullOrEmpty(city, nameof(city));
        Country = Guard.Against.NullOrEmpty(country, nameof(country));
        Phone = Guard.Against.NullOrEmpty(phone, nameof(phone));
        Email = Guard.Against.NullOrEmpty(email, nameof(email));
        PostalCode = postalCode;
        Fax = fax;
        Website = website;
        UpdateAuditInfo();
    }

    public void UpdatePolicies(
        TimeSpan checkInTime,
        TimeSpan checkOutTime,
        string? childPolicy = null,
        string? petPolicy = null,
        string? cancellationPolicy = null
    )
    {
        CheckInTime = checkInTime;
        CheckOutTime = checkOutTime;
        ChildPolicy = childPolicy;
        PetPolicy = petPolicy;
        CancellationPolicy = cancellationPolicy;
        UpdateAuditInfo();
    }

    public void UpdateConfiguration(string currency, string timeZone, string language)
    {
        Currency = Guard.Against.NullOrEmpty(currency, nameof(currency));
        TimeZone = Guard.Against.NullOrEmpty(timeZone, nameof(timeZone));
        Language = Guard.Against.NullOrEmpty(language, nameof(language));
        UpdateAuditInfo();
    }

    public void UpdateLogo(string logoUrl)
    {
        LogoUrl = logoUrl;
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
