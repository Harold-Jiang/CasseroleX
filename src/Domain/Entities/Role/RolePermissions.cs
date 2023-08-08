namespace CasseroleX.Domain.Entities.Role;


public partial class RolePermissions : BaseAuditableEntity
{
    // menu,file
    public string Type { get; set; } = null!;

    public int Pid { get; set; }

    public string Name { get; set; } = null!;

    public string? Title { get; set; }

    public string? Icon { get; set; }

    public string? Url { get; set; }

    public string? Condition { get; set; }

    public string? Remark { get; set; }

    public bool IsMenu { get; set; }

    public string? MenuType { get; set; }

    public string? Extend { get; set; }

    public string? Py { get; set; }

    public string? PinYin { get; set; }

    public int Weigh { get; set; }

    public Status Status { get; set; }
}
