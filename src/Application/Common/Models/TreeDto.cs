namespace CasseroleX.Application.Common.Models;
public abstract class TreeDto<T>
{
    public string Name { get; set; } = null!;
    public int Id { get; set; }

    public int Pid { get; set; }

    public List<T>? ChildList { get; set; }

    public int HasChild { get; set; }

    public string? Spacer { get; set; }

}
