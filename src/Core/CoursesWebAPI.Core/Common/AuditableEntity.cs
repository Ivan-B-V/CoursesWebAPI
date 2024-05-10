namespace CoursesWebAPI.Core.Common
{
    public abstract class AuditableEntity : Entity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

        public DateTimeOffset ModifiedAt { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
    }
}
