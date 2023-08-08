using CasseroleX.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace CasseroleX.Application.DashBoard.Queries;
public record GetDashBoardQuery : IRequest<DashBoardDto>;

public class GetDashBoardQueryHandler : IRequestHandler<GetDashBoardQuery, DashBoardDto>
{
    private readonly IApplicationDbContext _context;

    public GetDashBoardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashBoardDto> Handle(GetDashBoardQuery request, CancellationToken cancellationToken)
    {
        DashBoardDto dashBoard = new();

        DateTime startTime = DateTime.Now.AddDays(-6);
        DateTime endTime = DateTime.Now.Date;

        var joinList = await _context.Users
            .Where(user => user.JoinTime.HasValue && user.JoinTime >= startTime && user.JoinTime < endTime)
            .GroupBy(user => user.JoinTime.Value.Date)
            .Select(group => new
            {
                JoinDate = group.Key,
                NumUsers = group.Count()
            })
            .ToListAsync(cancellationToken);

        var column = Enumerable.Range(0, (int)(endTime - startTime).TotalDays + 1)
            .Select(offset => startTime.AddDays(offset).ToString("yyyy-MM-dd"))
            .ToList();

        dashBoard.UserList = column.ToDictionary(date => date, _ => 0);

        foreach (var v in joinList)
        {
            dashBoard.UserList[v.JoinDate.ToString("yyyy-MM-dd")] = v.NumUsers;
        }

        var dbTableList = _context.GetTableList();

        dashBoard.TotalUser = await _context.Users.CountAsync(cancellationToken);
        dashBoard.TotalAddon = 0;
        dashBoard.TotalWorkingAddon = 0;
        dashBoard.TotalAdmin = await _context.Admins.CountAsync(cancellationToken);
        dashBoard.TotalCategory = await _context.Categories.CountAsync(cancellationToken);
        dashBoard.TodayUserSignup = await _context.Users.CountAsync(user => EF.Functions.DateDiffDay(user.JoinTime, DateTime.Now) == 0, cancellationToken);
        dashBoard.TodayUserLogin = await _context.Users.CountAsync(user => EF.Functions.DateDiffDay(user.LoginTime, DateTime.Now) == 0, cancellationToken);
        dashBoard.SevenDAU = await _context.Users.CountAsync(user =>
        EF.Functions.DateDiffDay(user.JoinTime, DateTime.Now) >= -7 ||
        EF.Functions.DateDiffDay(user.LoginTime, DateTime.Now) >= -7 ||
        EF.Functions.DateDiffDay(user.PrevTime, DateTime.Now) >= -7, cancellationToken);
        dashBoard.ThirtyDAU = await _context.Users.CountAsync(user =>
        EF.Functions.DateDiffDay(user.JoinTime, DateTime.Now) >= -30 ||
        EF.Functions.DateDiffDay(user.LoginTime, DateTime.Now) >= -30 ||
        EF.Functions.DateDiffDay(user.PrevTime, DateTime.Now) >= -30, cancellationToken);
        dashBoard.ThreeDNU = await _context.Users.CountAsync(user => EF.Functions.DateDiffDay(user.JoinTime, DateTime.Now) >= -3, cancellationToken);
        dashBoard.SevenDNU = await _context.Users.CountAsync(user => EF.Functions.DateDiffDay(user.JoinTime, DateTime.Now) >= -7, cancellationToken);
        dashBoard.DbTableNums = dbTableList.Count;
        dashBoard.DbSize = dbTableList.Sum(item => item.Length);
        dashBoard.AttachmentNums = await _context.Attachments.CountAsync(cancellationToken);
        dashBoard.AttachmentSize = await _context.Attachments.SumAsync(attachment => attachment.FileSize, cancellationToken);
        dashBoard.PictureNums = await _context.Attachments.CountAsync(attachment => attachment.MimeType != null && attachment.MimeType.StartsWith("image/"), cancellationToken);
        dashBoard.PictureSize = await _context.Attachments.Where(attachment => attachment.MimeType != null && attachment.MimeType.StartsWith("image/")).SumAsync(attachment => attachment.FileSize, cancellationToken);

        return dashBoard;
    }
}
