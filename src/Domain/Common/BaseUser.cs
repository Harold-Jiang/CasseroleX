namespace CasseroleX.Domain.Common;
public class BaseUser : BaseAuditableEntity
{
    public string? UserName { get; set; }

    public string? NickName { get; set; }
   
    public string PasswordHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string? Avatar { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public int LoginFailure { get; set; }

    public DateTime? LoginTime { get; set; }

    public string? LoginIp { get; set; }

    public string? Token { get; set; }

    public Status Status { get; set; }

    public bool LockoutEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set;}

    public bool TwoFactorEnabled { get; set; }
}
