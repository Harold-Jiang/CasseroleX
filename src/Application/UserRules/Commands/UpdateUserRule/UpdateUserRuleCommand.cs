using AutoMapper;
using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;
using MediatR;

namespace CasseroleX.Application.UserRules.Commands.UpdateUserRule;
public class UpdateUserRuleCommand :  IRequest<Result>
{
    public int Id { get; set; }
    public int Pid { get; set; }

    public string Name { get; set; } = null!;

    public string? Title { get; set; }

    public string? Remark { get; set; }

    public bool IsMenu { get; set; }

    public int Weigh { get; set; }

    public Status Status { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateUserRuleCommand, UserRule>();
        }
    }
}

public class UpdateUserRuleCommandHandler : IRequestHandler<UpdateUserRuleCommand, Result>
{
    private readonly IApplicationDbContext _context; 
    private readonly IMapper _mapper;

    public UpdateUserRuleCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper; 
    }

    public async Task<Result> Handle(UpdateUserRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = await _context.UserRules.FindAsync(new object?[] { request.Id }, cancellationToken) ?? throw new NotFoundException(nameof(UserRule), request.Id);

        if (request.IsMenu && request.Pid == 0)
            throw new ArgumentException("The non-menu must have parent");

        if (request.Pid == request.Id)
            throw new ArgumentException("Can not change the parent to self");
         
        rule = _mapper.Map(request, rule);
        _context.UserRules.Update(rule);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;

        return result? Result.Success() : Result.Failure();
    }
}
