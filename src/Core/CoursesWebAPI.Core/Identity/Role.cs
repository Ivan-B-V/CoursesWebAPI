using Microsoft.AspNetCore.Identity;

namespace CoursesWebAPI.Core.Identity
{
    public sealed class Role : IdentityRole<Guid>
    {
        public Role() : base()
        { }

        public Role(string name) : base(name) 
        { }

        public ICollection<Permission> Permissions { get; set; } = new HashSet<Permission>();
    }
}