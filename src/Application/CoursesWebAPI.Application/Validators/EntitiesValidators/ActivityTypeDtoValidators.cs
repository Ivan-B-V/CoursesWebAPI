using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using FluentValidation;

namespace CoursesWebAPI.Application.Validators.EntitiesValidators
{
    public class ActivityTypeForCreatingDtoValidator : AbstractValidator<ActivityTypeForCreatingDto>
    {
        public ActivityTypeForCreatingDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                                .MaximumLength(20);
            RuleFor(x => x.Description).MaximumLength(100);
        }
    }

    public class ActivityTypeForUpdateDtoValidator : AbstractValidator<ActivityTypeForUpdateDto> 
    {
        public ActivityTypeForUpdateDtoValidator() 
        {
            RuleFor(x => x.Name).NotEmpty()
                                .MaximumLength(20);
            RuleFor(x => x.Description).MaximumLength(100);
        }
    }
}
