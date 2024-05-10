using CoursesWebAPI.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CoursesWebAPI.Infrastructure.Authorization;

public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
        _options = options.Value;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy is null && PolicyNameHelper.IsPolicyNameValid(policyName))
        {
            var permissions = PolicyNameHelper.GetPermissionsStringFrom(policyName);
            var requirement = new PermissonRequirement(permissions);

            policy = new AuthorizationPolicyBuilder()
                         .AddRequirements(requirement)
                         .Build();
            _options.AddPolicy(policyName, policy);
        }
        return policy;
    }
}
