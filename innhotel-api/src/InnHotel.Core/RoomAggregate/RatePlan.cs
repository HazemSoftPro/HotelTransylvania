using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.RoomAggregate;

/// <summary>
/// Represents different rate plans for room types (Flexible, Non-refundable, Bed & Breakfast, etc.)
/// </summary>
public class RatePlan : AuditableEntity, IAggregateRoot
{
    public int RoomTypeId { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; }
    public string? Description { get; private set; }
    public decimal BaseRate { get; private set; }
    public bool IsRefundable { get; private set; }
    public bool IncludesBreakfast { get; private set; }
    public bool IncludesTax { get; private set; }
    public int? MinimumStay { get; private set; }
    public int? MaximumStay { get; private set; }
    public DateTime? ValidFrom { get; private set; }
    public DateTime? ValidTo { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public RoomType RoomType { get; private set; } = null!;
    public ICollection<SeasonalRate> SeasonalRates { get; private set; } = new List<SeasonalRate>();

    private RatePlan() { } // EF Core constructor

    public RatePlan(
        int roomTypeId,
        string name,
        string code,
        decimal baseRate,
        bool isRefundable = true,
        bool includesBreakfast = false,
        bool includesTax = false,
        string? description = null
    )
    {
        RoomTypeId = Guard.Against.NegativeOrZero(roomTypeId, nameof(roomTypeId));
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Code = Guard.Against.NullOrEmpty(code, nameof(code));
        BaseRate = Guard.Against.NegativeOrZero(baseRate, nameof(baseRate));
        IsRefundable = isRefundable;
        IncludesBreakfast = includesBreakfast;
        IncludesTax = includesTax;
        Description = description;
        IsActive = true;
    }

    public void UpdateDetails(
        string name,
        string code,
        decimal baseRate,
        bool isRefundable,
        bool includesBreakfast,
        bool includesTax,
        string? description = null
    )
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        Code = Guard.Against.NullOrEmpty(code, nameof(code));
        BaseRate = Guard.Against.NegativeOrZero(baseRate, nameof(baseRate));
        IsRefundable = isRefundable;
        IncludesBreakfast = includesBreakfast;
        IncludesTax = includesTax;
        Description = description;
        UpdateAuditInfo();
    }

    public void UpdateStayRestrictions(int? minimumStay = null, int? maximumStay = null)
    {
        MinimumStay = minimumStay;
        MaximumStay = maximumStay;
        UpdateAuditInfo();
    }

    public void UpdateValidityPeriod(DateTime? validFrom = null, DateTime? validTo = null)
    {
        ValidFrom = validFrom;
        ValidTo = validTo;
        UpdateAuditInfo();
    }

    public void AddSeasonalRate(string name, DateTime startDate, DateTime endDate, decimal rate, decimal? multiplier = null)
    {
        var seasonalRate = new SeasonalRate(Id, name, startDate, endDate, rate, multiplier);
        SeasonalRates.Add(seasonalRate);
        UpdateAuditInfo();
    }

    public void RemoveSeasonalRate(int seasonalRateId)
    {
        var seasonalRate = SeasonalRates.FirstOrDefault(sr => sr.Id == seasonalRateId);
        if (seasonalRate != null)
        {
            SeasonalRates.Remove(seasonalRate);
            UpdateAuditInfo();
        }
    }

    public decimal GetRateForDate(DateTime date)
    {
        var seasonalRate = SeasonalRates
            .Where(sr => sr.IsActive && date >= sr.StartDate && date <= sr.EndDate)
            .OrderByDescending(sr => sr.CreatedAt)
            .FirstOrDefault();

        if (seasonalRate != null)
        {
            return seasonalRate.Multiplier.HasValue 
                ? BaseRate * seasonalRate.Multiplier.Value 
                : seasonalRate.Rate;
        }

        return BaseRate;
    }

    public bool IsValidForDate(DateTime date)
    {
        if (!IsActive) return false;
        if (ValidFrom.HasValue && date < ValidFrom.Value) return false;
        if (ValidTo.HasValue && date > ValidTo.Value) return false;
        return true;
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
