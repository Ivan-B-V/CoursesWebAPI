using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Common.Enums;

namespace CoursesWebAPI.Core.Entities
{
    public abstract class Person : Entity
    {
        protected Person() 
        {
            FirsName = string.Empty;
            LastName = string.Empty;
        }

        public string FirsName { get; set; }
        public string LastName { get; set; }
        public string? Patronomic { get; set; }
        public string? PhoneNumber { get; set; }
        public Sex Sex { get; set; }
        public Guid? UserId { get; set; }

        public override string ToString()
        {
            return string.Join(' ', FirsName, LastName, Patronomic);
        }
    }
}
