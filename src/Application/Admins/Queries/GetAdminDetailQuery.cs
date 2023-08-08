using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Admins.Queries;

public record GetAdminDetailQuery(int Id) : IRequest<AdminDto?>;

public class GetAdminDetailHandler : IRequestHandler<GetAdminDetailQuery, AdminDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IRoleManager _roleManager;
    private readonly IMapper _mapper;

    public GetAdminDetailHandler(IApplicationDbContext context,
        IMapper mapper,
        IRoleManager roleManager)
    {
        _context = context;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<AdminDto?> Handle(GetAdminDetailQuery request, CancellationToken cancellationToken)
    {
        var admin = await _context.Admins
            .ProjectTo<AdminDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken);

        if (admin is not null)
        {
           var adminRoles = await _roleManager.GetRolesAsync(admin.Id, cancellationToken);
            admin.RoleIds = string.Join(",", adminRoles.Select(s => s.Id).ToList());
        }
        return admin;
    }

}
