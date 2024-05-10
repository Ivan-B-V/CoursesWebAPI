using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using Riok.Mapperly.Abstractions;

namespace CoursesWebAPI.Application.Mappers
{
    [Mapper]
    [UseStaticMapper(typeof(PersonMapper))]
    public static partial class EmployeeMapper
    {
        public static partial Employee ToEmployee(this EmployeeForCreatingDto source);
        public static partial void Map(EmployeeForUpdateDto source, Employee target);
        public static partial EmployeeDto ToEmployeeDto(this Employee source);
    }
}
