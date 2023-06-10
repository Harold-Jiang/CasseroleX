using CasseroleX.Application.Login.Commands;

namespace CasseroleX.Application.Common.Interfaces;
public interface IAdminManager
{
    Task<AdminLoginResultDto> Login(AdminLoginCommand model,CancellationToken cancellationToken = default);

}
