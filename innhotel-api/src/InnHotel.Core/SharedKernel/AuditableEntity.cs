namespace InnHotel.Core.SharedKernel;

/// <summary>
/// Base class for entities that require audit trail functionality
/// </summary>
public abstract class AuditableEntity : EntityBase
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    protected AuditableEntity()
    {
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public virtual void MarkAsDeleted(string? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }

    public virtual void UpdateAuditInfo(string? updatedBy = null)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
