namespace CoursesWebAPI.Core.Entities
{
    public sealed class Employee : Person
    {
        public Employee()
        {
            Activities = new HashSet<Activity>();
        }

        public ICollection<Activity> Activities { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
