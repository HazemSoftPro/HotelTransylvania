using InnHotel.Core.GuestAggregate.ValueObjects;
using InnHotel.Core.SharedKernel;

namespace InnHotel.Core.GuestAggregate;

public class Guest : AuditableEntity, IAggregateRoot
{
  // ========================
  // properties
  // ========================
  public string FirstName { get; private set; }
  public string LastName { get; private set; }
  public Gender Gender { get; private set; }
  public IdProofType IdProofType { get; private set; }
  public string IdProofNumber { get; private set; }
  public DateTime? IdProofExpiryDate { get; private set; }
  public string? Email { get; private set; }
  public string? Phone { get; private set; }
  public string? Address { get; private set; }
  public string? Nationality { get; private set; }
  public DateTime? DateOfBirth { get; private set; }
  public GuestCategory Category { get; private set; }
  public string? PhotoUrl { get; private set; }
  public string? IdDocumentUrl { get; private set; }
  public string? Notes { get; private set; }

  // Navigation properties
  public ICollection<GuestPreference> Preferences { get; private set; } = new List<GuestPreference>();
  public ICollection<GuestCommunication> Communications { get; private set; } = new List<GuestCommunication>();

  // ========================
  // constructor
  // ========================
  private Guest() { } // EF Core constructor

  public Guest(
    string firstName,
    string lastName,
    Gender gender,
    IdProofType idProofType,
    string idProofNumber,
    DateTime? idProofExpiryDate = null,
    string? email = null,
    string? phone = null,
    string? address = null,
    string? nationality = null,
    DateTime? dateOfBirth = null,
    GuestCategory category = GuestCategory.Regular
)
  {
    FirstName = Guard.Against.NullOrEmpty(firstName, nameof(firstName));
    LastName = Guard.Against.NullOrEmpty(lastName, nameof(lastName));
    Gender = gender;
    IdProofType = idProofType;
    IdProofNumber = Guard.Against.NullOrEmpty(idProofNumber, nameof(idProofNumber));
    IdProofExpiryDate = idProofExpiryDate;
    Email = email;
    Phone = phone;
    Address = address;
    Nationality = nationality;
    DateOfBirth = dateOfBirth;
    Category = category;
  }

  // ========================
  // update methods
  // ========================
  public void UpdateDetails(
      string firstName,
      string lastName,
      Gender gender,
      IdProofType idProofType,
      string idProofNumber,
      DateTime? idProofExpiryDate = null,
      string? email = null,
      string? phone = null,
      string? address = null,
      string? nationality = null,
      DateTime? dateOfBirth = null
  )
  {
    FirstName = Guard.Against.NullOrEmpty(firstName, nameof(firstName));
    LastName = Guard.Against.NullOrEmpty(lastName, nameof(lastName));
    Gender = gender;
    IdProofType = idProofType;
    IdProofNumber = Guard.Against.NullOrEmpty(idProofNumber, nameof(idProofNumber));
    IdProofExpiryDate = idProofExpiryDate;
    Email = email;
    Phone = phone;
    Address = address;
    Nationality = nationality;
    DateOfBirth = dateOfBirth;
    UpdateAuditInfo();
  }

  public void UpdateCategory(GuestCategory category)
  {
    Category = category;
    UpdateAuditInfo();
  }

  public void UpdatePhoto(string photoUrl)
  {
    PhotoUrl = photoUrl;
    UpdateAuditInfo();
  }

  public void UpdateIdDocument(string idDocumentUrl)
  {
    IdDocumentUrl = idDocumentUrl;
    UpdateAuditInfo();
  }

  public void UpdateNotes(string? notes)
  {
    Notes = notes;
    UpdateAuditInfo();
  }

  public void AddPreference(string type, string value, string? description = null)
  {
    var preference = new GuestPreference(Id, type, value, description);
    Preferences.Add(preference);
    UpdateAuditInfo();
  }

  public void RemovePreference(int preferenceId)
  {
    var preference = Preferences.FirstOrDefault(p => p.Id == preferenceId);
    if (preference != null)
    {
      Preferences.Remove(preference);
      UpdateAuditInfo();
    }
  }

  public void AddCommunication(string type, string subject, string content, string? sentBy = null)
  {
    var communication = new GuestCommunication(Id, type, subject, content, sentBy);
    Communications.Add(communication);
    UpdateAuditInfo();
  }

  public string FullName => $"{FirstName} {LastName}";
  public int Age => DateOfBirth.HasValue ? DateTime.Today.Year - DateOfBirth.Value.Year : 0;
}
