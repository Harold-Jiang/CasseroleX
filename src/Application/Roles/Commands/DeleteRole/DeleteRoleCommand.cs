using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Domain.Entities.Role;
using MediatR;

namespace CasseroleX.Application.Roles.Commands.DeleteRole;
public record DeleteRoleCommand(int Id) : IRequest;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteRoleCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Role), request.Id);
        }

        _context.Roles.Remove(entity);

        //entity.AddDomainEvent(new TodoItemDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

    }
}
