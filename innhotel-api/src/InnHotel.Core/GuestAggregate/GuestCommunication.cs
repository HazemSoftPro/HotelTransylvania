using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.GuestAggregate;

/// <summary>
/// Represents communication history with a guest (emails, calls, notes)
/// </summary>
public class GuestCommunication : AuditableEntity
{
    public int GuestId { get; private set; }
    public string Type { get; private set; } // Email, Phone, Note, SMS
    public string Subject { get; private set; }
    public string Content { get; private set; }
    public DateTime CommunicationDate { get; private set; }
    public string? SentBy { get; private set; }
    public bool IsInbound { get; private set; }

    // Navigation properties
    public Guest Guest { get; private set; } = null!;

    private GuestCommunication() { } // EF Core constructor

    public GuestCommunication(int guestId, string type, string subject, string content, string? sentBy = null, bool isInbound = false)
    {
        GuestId = Guard.Against.NegativeOrZero(guestId, nameof(guestId));
        Type = Guard.Against.NullOrEmpty(type, nameof(type));
        Subject = Guard.Against.NullOrEmpty(subject, nameof(subject));
        Content = Guard.Against.NullOrEmpty(content, nameof(content));
        SentBy = sentBy;
        IsInbound = isInbound;
        CommunicationDate = DateTime.UtcNow;
    }

    public void UpdateContent(string subject, string content)
    {
        Subject = Guard.Against.NullOrEmpty(subject, nameof(subject));
        Content = Guard.Against.NullOrEmpty(content, nameof(content));
        UpdateAuditInfo();
    }
}

public static class CommunicationTypes
{
    public const string Email = "Email";
    public const string Phone = "Phone";
    public const string Note = "Note";
    public const string SMS = "SMS";
    public const string InPerson = "InPerson";
}
