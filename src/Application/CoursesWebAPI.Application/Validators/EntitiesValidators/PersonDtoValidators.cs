using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using FluentValidation;

namespace CoursesWebAPI.Application.Validators.EntitiesValidators
{
    public class PersonDtoValidator<T> : AbstractValidator<T> where T : PersonDto
    {
        public PersonDtoValidator() 
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.FirsName).NotEmpty().MaximumLength(30);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Patronomic).MaximumLength(30);
            RuleFor(x => x.PhoneNumber).Matches(@"(^[+])(\d{2,3})(\d{2,3})(\d{7})");
        }
    }

    public class PersonForCreatingDtoValidator<T> : AbstractValidator<T> where T : PersonForCreatingDto
    {
        public PersonForCreatingDtoValidator()
        {
            RuleFor(x => x.FirsName).NotEmpty().MaximumLength(30);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Patronomic).MaximumLength(30);
            RuleFor(x => x.PhoneNumber).Matches(@"(^[+])(\d{2,3})(\d{2,3})(\d{7})");
        }
    }

    public class PersonForUpdateDtoValidator<T> : AbstractValidator<T> where T : PersonForUpdateDto
    {
        public PersonForUpdateDtoValidator()
        {
            RuleFor(x => x.FirsName).NotEmpty().MaximumLength(30);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Patronomic).MaximumLength(30);
            RuleFor(x => x.PhoneNumber).Matches(@"(^[+])(\d{2,3})(\d{2,3})(\d{7})");
        }
    }
}
