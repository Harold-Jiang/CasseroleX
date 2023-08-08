using AutoMapper;
using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Caching.Constants;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Menus.Commands.CreateMenuCommand;
public class CreateMenuCommand :  IRequest<Result>
{
    public int Pid { get; set; }
    public string Name { get; set; } = null!; 
    public string? Title { get; set; } 
    public string? Remark { get; set; } 
    public bool IsMenu { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; } 
    public string? Condition { get; set; } 
    public int Weigh { get; set; }
    public string? MenuType { get; set; } 
    public string? Extend { get; set; } 
    public Status Status { get; set; }
     

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateMenuCommand, RolePermissions>();
        }
    }
}
public class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;

    public CreateMenuCommandHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        if (request.IsMenu && request.Pid == 0) 
            throw new ArgumentException("The non-menu must have parent");
        
        var menu = _mapper.Map<RolePermissions>(request);

        await _context.RolePermissions.AddAsync(menu, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;

        if (!result)
            return Result.Failure();

        var admins = await _context.Admins.Select(x => x.Id).ToListAsync(cancellationToken);
        foreach (var admin in admins)
        {
            //remove cache
            await _cache.RemoveAsync(string.Format(CacheKeys.ADMIN_ROLEPERMISSIONS_BY_ADMINID_KEY, admin), cancellationToken);
        }
        return Result.Success();
    }
}
