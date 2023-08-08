using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Caching.Constants;
using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Menus.Queries;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Menus.Commands.UpdateMenuCommand;
public class UpdateMenuCommand : IRequest<Result>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Title { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public string? Badge { get; set; }
    public string? MenuClass { get; set; }
    public string? MenuTabs { get; set; }
    public string? MenuType { get; set; }
    public bool IsMenu { get; set; }
    public string? Extend { get; set; }
    public int Weigh { get; set; }
    public Status Status { get; set; }
    public string? Condition { get; set; }
    public string? Remark { get; set; }

    public int Pid { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateMenuCommand, RolePermissions>();
        }
    }
}

public class UpdateMenuCommandHandler : IRequestHandler<UpdateMenuCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;

    public UpdateMenuCommandHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
    {
        var menu = await _context.RolePermissions.FindAsync(new object?[] { request.Id }, cancellationToken) ?? throw new NotFoundException("Menu", request.Id);

        if (!request.IsMenu && request.Pid == 0)
            throw new ArgumentException("The non-menu must have parent");

        if (request.Pid == request.Id)
            throw new ArgumentException("Can not change the parent to self");

        if (request.Pid != menu.Pid)
        {
            var rolePermissions = await _context.RolePermissions
                .Select(s => new RolePermissions { Id = s.Id, Pid = s.Pid })
                .ProjectTo<MenuDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var childrenIds = Tree.GetChildrenIds(rolePermissions, request.Id);
            if (childrenIds.Contains(request.Pid))
            {
                throw new ArgumentException("Can not change the parent to child");
            }
        }
        menu = _mapper.Map(request, menu);
        _context.RolePermissions.Update(menu);
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
