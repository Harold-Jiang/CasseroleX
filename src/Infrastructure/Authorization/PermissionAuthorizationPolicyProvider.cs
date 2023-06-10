using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;

namespace CasseroleX.Infrastructure.Authorization;
public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        return await base.GetPolicyAsync(policyName)
                ?? new AuthorizationPolicyBuilder()
                    .AddRequirements(new OperationAuthorizationRequirement() { Name = policyName })
                    .Build();
    }
}
