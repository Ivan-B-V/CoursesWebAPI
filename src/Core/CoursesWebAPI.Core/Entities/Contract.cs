using CoursesWebAPI.Core.Common;

namespace CoursesWebAPI.Core.Entities
{
    public class Contract : Entity
    {
        public string Number { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public short Hours { get; set; }
        public short PaidHours { get; set; }
        public short PassedHours { get; set; }
        public bool Closed { get; set; }
        public DateTimeOffset Concluded { get; set; }
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
        public ICollection<Activity> Activities { get; private set; } = new HashSet<Activity>();
    }
}