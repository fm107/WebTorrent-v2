using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebTorrent.Data.Core;

namespace WebTorrent.Web.Authorization
{
    public class ViewRoleAuthorizationRequirement : IAuthorizationRequirement
    {
    }


    public class ViewRoleAuthorizationHandler : AuthorizationHandler<ViewRoleAuthorizationRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ViewRoleAuthorizationRequirement requirement, string roleName)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            if (context.User.HasClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewRoles) ||
                context.User.IsInRole(roleName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}