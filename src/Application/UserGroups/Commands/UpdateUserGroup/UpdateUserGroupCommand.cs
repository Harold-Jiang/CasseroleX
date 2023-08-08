using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;
using MediatR;

namespace CasseroleX.Application.UserGroups.Commands.UpdateUserGroup;
public class UpdateUserGroupCommand : IRequest<Result>
{
    public int Id { get; set; } 
    public string Rules { get; init; } = null!;
    public string Name { get; set; } = null!;
    public Status Status { get; set; }
}

public class UpdateUserGroupCommandHandler : IRequestHandler<UpdateUserGroupCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateUserGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.UserGroups
           .FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException(nameof(UserGroup), request.Id);

        entity.Rules = string.Join(",", request.Rules);
        entity.Name = request.Name; 
        entity.Status = request.Status;

        _context.UserGroups.Update(entity);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;
        return result ? Result.Success() : Result.Failure();
    }
}
