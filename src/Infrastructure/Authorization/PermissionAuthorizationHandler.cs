using System.Security.Claims;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CasseroleX.Infrastructure.Authorization;
public class PermissionAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value.ToInt() ?? 0;
        if (userId == 0)
        {
            return;
        }
        using var scope =  _serviceScopeFactory.CreateScope();
        IRoleManager roleManager = scope.ServiceProvider.GetRequiredService<IRoleManager>();

        HashSet<string> permissions = await roleManager.GetPermissionsAsync(userId);
        if (permissions.Contains(requirement.Name))
        {
            context.Succeed(requirement);
        }

    }
}
