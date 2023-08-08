using Microsoft.AspNetCore.Authorization;

namespace CasseroleX.Infrastructure.Authorization;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
public class HasPermissionAttribute : AuthorizeAttribute
{ 
    public HasPermissionAttribute(string perssionName = "none"):base(policy: perssionName)
    {
    }
}
