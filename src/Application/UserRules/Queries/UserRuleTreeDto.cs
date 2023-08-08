namespace CasseroleX.Application.UserRules.Queries;
public class UserRuleTreeDto
{
    public int Id { get; set; }
    public string? Parent { get; set; }
    public string? Text { get; set; }
    public string Type { get; set; } = "menu";
    public Dictionary<string, bool>? State { get; set; }

}
