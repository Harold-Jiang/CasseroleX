using AutoMapper;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;
using MediatR;

namespace CasseroleX.Application.UserRules.Commands.CreateUserRule;

public class CreateUserRuleCommand : IRequest<Result>
{
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
            CreateMap<CreateUserRuleCommand, UserRule>();
        }
    }
}

public class CreateUserRuleCommandHanlder : IRequestHandler<CreateUserRuleCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateUserRuleCommandHanlder(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<Result> Handle(CreateUserRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = _mapper.Map<UserRule>(request);

        await _context.UserRules.AddAsync(rule, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;
         
        return result? Result.Success(): Result.Failure();
    }
}