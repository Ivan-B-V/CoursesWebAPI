using CoursesWebAPI.Core.Common.Enums;

namespace CoursesWebAPI.Shared.Authorization
{
    public class HasPermissionAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public HasPermissionAttribute(string policy) : base(policy) { }

        public HasPermissionAttribute(params Permissions[] permissions)
        {
            Permissions = permissions.ToHashSet();   
        }

        public HashSet<Permissions> Permissions
        {
            get
            {
                return !string.IsNullOrEmpty(Policy)
                    ? PolicyNameHelper.GetPermissionsFrom(Policy) 
                    : new HashSet<Permissions>();
            }
            set
            {
                Policy = value.Any()
                    ? PolicyNameHelper.GeneratePolicyNameFrom(value) 
                    : string.Empty;
            }
        }
    }
}
