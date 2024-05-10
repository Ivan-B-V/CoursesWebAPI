using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CoursesWebAPI.Infrastructure.Authorization;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissonRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissonRequirement requirement)
    {
        if (context.User is null)
        {
            return Task.CompletedTask;
        }

        var userPermissions = context.User.Claims.Where(c => c.Type.Equals(CustomClaimTypes.Permisson))
                                                 .Select(c => Enum.Parse<Permissions>(c.Value));

        if (!userPermissions.Any())
        {
            return Task.CompletedTask;
        }

        if (userPermissions.Intersect(requirement.Permissions).Any())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}
