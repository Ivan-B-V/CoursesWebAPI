using CoursesWebAPI.Core.Common.Enums;
using System.Text;

namespace CoursesWebAPI.Shared.Authorization;

public static class PolicyNameHelper
{
    public const string Prefix = "Permissions";

    public static bool IsPolicyNameValid(string policyName)
    {
        if (policyName is not null &&
            policyName.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    public static HashSet<Permissions> GetPermissionsFrom(string policyName)
    {
        if (string.IsNullOrEmpty(policyName))
        {
            return new HashSet<Permissions>();
        }

        if (policyName.StartsWith(Prefix))
        {
            var permissionsInString = policyName[Prefix.Length..];
            return GetPermissionsFrom(permissionsInString);
        }
        return GetPermissionsFromString(policyName);
    }

    private static HashSet<Permissions> GetPermissionsFromString(string permissionsString)
    {
        var permissionsInString = permissionsString.Split('|');
        return permissionsInString.Select(p => (Permissions)Enum.Parse(typeof(Permissions), p)).ToHashSet();
    }

    public static string GetPermissionsStringFrom(string policyName)
    {
        if (policyName.Length < Prefix.Length)
        {
            return string.Empty;
        }    
        return policyName[Prefix.Length..];
    }

    public static string GeneratePolicyNameFrom(IEnumerable<Permissions> permissions)
    {
        StringBuilder stringBuilder = new(Prefix);
        stringBuilder.AppendJoin('|', permissions);
        return stringBuilder.ToString();
    }
}
