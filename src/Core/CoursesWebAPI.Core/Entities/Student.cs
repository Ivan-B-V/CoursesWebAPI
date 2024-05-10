namespace CoursesWebAPI.Core.Entities
{
    public sealed class Student : Person
    {
        public Student()
        {
            Contracts = new HashSet<Contract>();
            Activities = new HashSet<Activity>();
        }

        public ICollection<Contract> Contracts { get; private set; }
        public ICollection<Activity> Activities { get; private set; }
    }
}