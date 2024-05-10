using CoursesWebAPI.Core.Common;

namespace CoursesWebAPI.Core.Entities
{
    public class Activity : Entity
    {
        public string? Description { get; set; }
        public DateTimeOffset Begin { get; set; }
        public DateTimeOffset End { get; set; }

        public Guid ActivityTypeId { get; set; }
        public ActivityType? ActivityType { get; set; }
        
        public Guid? TeacherId { get; set;}
        public Employee? Teacher {get; set;}
        
        public ICollection<Contract> Contracts {get; set;} = new HashSet<Contract>();
    }
}
