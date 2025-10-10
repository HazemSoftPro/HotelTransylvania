using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.RoomAggregate;

/// <summary>
/// Represents seasonal pricing for rate plans (Summer rates, Holiday rates, etc.)
/// </summary>
public class SeasonalRate : AuditableEntity
{
    public int RatePlanId { get; private set; }
    public string Name { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal Rate { get; private set; }
    public decimal? Multiplier { get; private set; } // Alternative to fixed rate - multiply base rate
    public bool IsActive { get; private set; }

    // Navigation properties
    public RatePlan RatePlan { get; private set; } = null!;

    private SeasonalRate() { } // EF Core constructor

    public SeasonalRate(
        int ratePlanId,
        string name,
        DateTime startDate,
        DateTime endDate,
        decimal rate,
        decimal? multiplier = null
    )
    {
        RatePlanId = Guard.Against.NegativeOrZero(ratePlanId, nameof(ratePlanId));
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        StartDate = startDate;
        EndDate = Guard.Against.OutOfRange(endDate, nameof(endDate), startDate, DateTime.MaxValue);
        Rate = Guard.Against.NegativeOrZero(rate, nameof(rate));
        Multiplier = multiplier;
        IsActive = true;
    }

    public void UpdateDetails(
        string name,
        DateTime startDate,
        DateTime endDate,
        decimal rate,
        decimal? multiplier = null
    )
    {
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        StartDate = startDate;
        EndDate = Guard.Against.OutOfRange(endDate, nameof(endDate), startDate, DateTime.MaxValue);
        Rate = Guard.Against.NegativeOrZero(rate, nameof(rate));
        Multiplier = multiplier;
        UpdateAuditInfo();
    }

    public bool IsValidForDate(DateTime date)
    {
        return IsActive && date >= StartDate && date <= EndDate;
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
