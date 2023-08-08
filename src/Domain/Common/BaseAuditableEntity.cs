namespace CasseroleX.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public int LastModifiedBy { get; set; }
    public int CreatedBy { get; set; }
}
