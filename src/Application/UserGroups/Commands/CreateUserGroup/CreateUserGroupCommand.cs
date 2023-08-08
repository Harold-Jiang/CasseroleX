using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;
using MediatR;

namespace CasseroleX.Application.UserGroups.Commands.CreateUserGroup;
public class CreateUserGroupCommand : IRequest<Result>
{ 
    public string Rules { get; init; } = null!;
    public string Name { get; set; } = null!;
    public Status Status { get; set; } 
}
public class CreateUserGroupCommandHandler : IRequestHandler<CreateUserGroupCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CreateUserGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CreateUserGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new UserGroup
        {
            Rules = string.Join(",", request.Rules),
            Name = request.Name,
            Status = request.Status
        };

        await _context.UserGroups.AddAsync(entity, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;
        return result ? Result.Success(entity.Id) : Result.Failure();
    }
}
