using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using Riok.Mapperly.Abstractions;

namespace CoursesWebAPI.Application.Mappers
{
    [Mapper]
    [UseStaticMapper(typeof(PersonMapper))]
    public static partial class StudentMapper
    {
        public static partial StudentDto ToStudentDto(this Student source);
        public static partial StudentForUpdateDto ToStudentForUpdateDto(this Student source);
        public static ICollection<StudentForUpdateDto> ToStudentDtos(this ICollection<Student> source) =>
            source.Select(s => s.ToStudentForUpdateDto()).ToHashSet();
        public static partial Student ToStudent(this StudentForUpdateDto source);
        public static partial Student ToStudent(this StudentForCreatingDto source);
    }
}
