
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using FluentValidation;

namespace CoursesWebAPI.Application.Validators.EntitiesValidators
{
    public sealed class EmployeeForCreatingDtoValidator : PersonForCreatingDtoValidator<EmployeeForCreatingDto>
    {
        public EmployeeForCreatingDtoValidator()
        {
        }
    }

    public sealed class EmployeeForUpdateDtoValidator : PersonForUpdateDtoValidator<EmployeeForUpdateDto>
    {
        public EmployeeForUpdateDtoValidator()
        {
        }
    }
}
