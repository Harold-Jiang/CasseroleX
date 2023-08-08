namespace CasseroleX.Application.Roles.Queries;
public class RoleTreeDto
{
    public int Id { get; set; }
    public string? Parent { get; set; }
    public string? Text { get; set; }
    public string Type { get; set; } = "menu";
    public Dictionary<string, bool>? State { get; set; }

}
