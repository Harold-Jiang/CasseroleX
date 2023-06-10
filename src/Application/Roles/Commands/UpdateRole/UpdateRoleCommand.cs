using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;
using MediatR;

namespace CasseroleX.Application.Roles.Commands.UpdateRole;
public record UpdateRoleCommand : IRequest<bool>
{
    public int Id { get; init; }

    public int Pid { get; init; }
    public string Rules { get; init; } = null!;
    public string Name { get; set; } = null!;
    public Status Status { get; set; }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateRoleCommand,bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Role), request.Id);
        }

        entity.Rules = request.Rules;
        entity.Name = request.Name;
        entity.Pid = request.Pid;
        entity.Status = request.Status;

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

   
}
