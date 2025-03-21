using System.Security.Claims;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace CasseroleX.Infrastructure.Authorization;
public class PermissionAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value.ToInt() ?? 0;
        if (userId == 0)
        {
            return;
        }
        using var scope = _serviceScopeFactory.CreateScope();
        IRoleManager roleManager = scope.ServiceProvider.GetRequiredService<IRoleManager>();

        HashSet<string> permissions = await roleManager.GetPermissionsAsync(userId);

        if (requirement.Name == "none" && _httpContextAccessor.HttpContext != null)
        {
            var endpoint = _httpContextAccessor.HttpContext.GetEndpoint();
            if (endpoint is RouteEndpoint routeEndpoint)
            {
                var routeAttribute = routeEndpoint.Metadata.OfType<RouteAttribute>().SingleOrDefault();
                if (routeAttribute != null &&
                    routeEndpoint.RoutePattern.RequiredValues.TryGetValue("action", out var actionValue))
                {
                    requirement.Name = $"{routeAttribute.Template}/{actionValue?.ToString()}";
                }
            }
        }

        if (permissions.Contains(requirement.Name.ToLowerInvariant()))
        {
            context.Succeed(requirement);
        }

    }
}
