using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Menus.Queries;
public record GetMenusQuery : IRequest<List<MenuDto>>;

public class GetMenusHandler : IRequestHandler<GetMenusQuery, List<MenuDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMenusHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<MenuDto>> Handle(GetMenusQuery request, CancellationToken cancellationToken)
    {
        var menuList = await _context.RolePermissions
                      .OrderByDescending(r => r.Weigh) 
                      .ProjectTo<MenuDto>(_mapper.ConfigurationProvider)
                      .ToListAsync(cancellationToken);

        return  Tree.GetTreeList(Tree.GetTreeArray(menuList, 0), "Title");
    }

}
