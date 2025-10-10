namespace InnHotel.Core.GuestAggregate.ValueObjects;

public class GuestCategory : SmartEnum<GuestCategory>
{
    public static readonly GuestCategory Regular = new(nameof(Regular), 1);
    public static readonly GuestCategory Frequent = new(nameof(Frequent), 2);
    public static readonly GuestCategory VIP = new(nameof(VIP), 3);
    public static readonly GuestCategory LoyaltyMember = new(nameof(LoyaltyMember), 4);
    public static readonly GuestCategory Corporate = new(nameof(Corporate), 5);

    protected GuestCategory(string name, int value) : base(name, value) { }
}
