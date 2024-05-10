using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using FluentValidation;

namespace CoursesWebAPI.Application.Validators.EntitiesValidators
{
    public sealed class RoleForCreatingDtoValidator : AbstractValidator<RoleForCreatingDto>
    {
        public RoleForCreatingDtoValidator()
        {
            RuleFor(r => r.Name).NotEmpty()
                                .MaximumLength(50);
        }
    }

    public sealed class RoleForUpdateDtoValidator : AbstractValidator<RoleForUpdateDto> 
    {
        public RoleForUpdateDtoValidator()
        {
            RuleFor(r => r.Name).NotEmpty()
                                .MaximumLength(50);
        }
    }
}
