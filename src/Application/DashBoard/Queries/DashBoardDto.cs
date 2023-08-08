namespace CasseroleX.Application.DashBoard.Queries;
public class DashBoardDto
{
    public int TotalUser { get; set; }
    public int TotalAddon { get; set; }
    public int TotalWorkingAddon { get; set; }
    public int TotalAdmin { get; set; }
    public int TotalCategory { get; set; }
    public int TodayUserSignup { get; set; }
    public int TodayUserLogin { get; set; }
    public int SevenDAU { get; set; }
    public int ThirtyDAU { get; set; }
    public int ThreeDNU { get; set; }
    public int SevenDNU { get; set; }
    public int DbTableNums { get; set; }
    public int DbSize { get; set; }
    public int AttachmentNums { get; set; }
    public int AttachmentSize { get; set; }
    public int PictureNums { get; set; }
    public int PictureSize { get; set; }

    public Dictionary<string, int>? UserList { get; set; }
}
