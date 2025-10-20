namespace InnHotel.Core.PaymentAggregate;

/// <summary>
/// Represents the method used for payment
/// </summary>
public enum PaymentMethod
{
    Cash = 1,
    CreditCard = 2,
    DebitCard = 3,
    BankTransfer = 4,
    OnlinePayment = 5,
    MobilePayment = 6,
    Check = 7
}