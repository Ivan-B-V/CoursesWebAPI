using CoursesWebAPI.Core.Common;

namespace CoursesWebAPI.Core.Identity
{
    public class RefreshToken : Entity
    {
        public Guid UserId { get; set; }
        public Guid JwtId { get; set; }
        public Guid Value => Id;
        public string Fingerprint { get; set; } = string.Empty;
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}