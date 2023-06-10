namespace CasseroleX.Domain.Entities;
public class Category : BaseAuditableEntity
{
    public int Pid { get; set; }

    public string? Name { get; set; }

    public string? NickName { get; set; }

    public string Type { get; set; } = null!;

    public Status Status { get; set; }
}
