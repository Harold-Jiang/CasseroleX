using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Mappings;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Users.Queries;

public class GetUsersQueryHandler : IRequestHandler<SearchQuery<User,UserDto>, PaginatedList<UserDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUploadService _uploadService;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(IApplicationDbContext context,
        IMapper mapper,
        IUploadService uploadService)
    {
        _context = context;
        _mapper = mapper;
        _uploadService = uploadService;
    }

    public async Task<PaginatedList<UserDto>> Handle(SearchQuery<User, UserDto> request, CancellationToken cancellationToken)
    {  

        var users = await _context.Users
            .Include(x=>x.Group)
            .Where(request.GetQueryLamda())
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit);

        foreach (var user in users.Items)
        {
            user.Avatar = !user.Avatar.IsNotNullOrWhiteSpace()? user.UserName.LetterAvatar() :  _uploadService.GetFullUrl(user.Avatar);
        }
        return users;

    }
}
