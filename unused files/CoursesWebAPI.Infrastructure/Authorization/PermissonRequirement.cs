using CoursesWebAPI.Core.Common.Enums;
using CoursesWebAPI.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace CoursesWebAPI.Infrastructure.Authorization;

public sealed class PermissonRequirement : IAuthorizationRequirement
{
    public PermissonRequirement(string permissions)
    {
        ArgumentException.ThrowIfNullOrEmpty(permissions);
        Permissions = PolicyNameHelper.GetPermissionsFrom(permissions);
    }

    public IEnumerable<Permissions> Permissions { get; }
}
