namespace CasseroleX.Application.Login.Commands;
public class AdminLoginResultDto
{
    public string? Url { get; set; }
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? Avatar { get; set; }
}
