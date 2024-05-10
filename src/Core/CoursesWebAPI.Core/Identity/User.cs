using CoursesWebAPI.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace CoursesWebAPI.Core.Identity
{
    public sealed class User : IdentityUser<Guid>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="User"/>.
        /// </summary>
        /// <remarks>
        /// The Id property is initialized to form a new GUID string value.
        /// </remarks>
        public User() : base()
        {
            Roles = new HashSet<Role>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="User"/>.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <remarks>
        /// The Id property is initialized to form a new GUID string value.
        /// </remarks>
        public User(string userName) : this()
        {
            UserName = userName;
        }

        public Guid? PersonId { get; set; }
        public Person? Person { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; private set; }
        public ICollection<Role> Roles { get; private set; }

    }
}
