using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Menus.Queries;
public record GetMenuDetailQuery(int Id) : IRequest<MenuDto?>;
public class GetMenuDetailHandler : IRequestHandler<GetMenuDetailQuery, MenuDto?>
{
    private readonly IApplicationDbContext _context; 
    private readonly IMapper _mapper;

    public GetMenuDetailHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MenuDto?> Handle(GetMenuDetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.RolePermissions
            .ProjectTo<MenuDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken); 
    }

}
