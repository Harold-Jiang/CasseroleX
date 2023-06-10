using CasseroleX.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

namespace CasseroleX.Application.Login.Commands;
public class AdminLoginCommand : IRequest<AdminLoginResultDto>
{ 
    public string UserName { get; set; } = null!; 
    public string Password { get; set; } = null!; 
    public bool RememberMe { get; set; }
    public string? Code { get; set; }
}
// 定义登录命令处理器
public class LoginCommandHandler : IRequestHandler<AdminLoginCommand, AdminLoginResultDto>
{
    private readonly IAdminManager _accountService;

    public LoginCommandHandler(IAdminManager accountService)
    {
        _accountService = accountService;
    }

    public async Task<AdminLoginResultDto> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
    {
        return  await _accountService.Login(request,cancellationToken);
    }
}

public class LoginCommandValidator : AbstractValidator<AdminLoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();

    }
}