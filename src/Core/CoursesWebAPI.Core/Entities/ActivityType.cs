using CoursesWebAPI.Core.Common;

namespace CoursesWebAPI.Core.Entities
{
    public sealed class ActivityType : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public override string ToString() => Name;
    }
}
