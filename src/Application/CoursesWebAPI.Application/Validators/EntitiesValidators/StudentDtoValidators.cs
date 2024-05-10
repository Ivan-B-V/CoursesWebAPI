using CoursesWebAPI.Shared.DataTransferObjects.Entities;

namespace CoursesWebAPI.Application.Validators.EntitiesValidators
{
    public sealed class StudentDtoValidator : PersonDtoValidator<StudentDto>
    {
        public StudentDtoValidator()
        {
        }
    }

    public sealed class StudentForCreatingDtoValidator : PersonForCreatingDtoValidator<StudentForCreatingDto>
    {
        public StudentForCreatingDtoValidator()
        {
        }
    }
}
