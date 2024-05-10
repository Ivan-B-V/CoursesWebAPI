using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using FluentValidation;

namespace CoursesWebAPI.Application.Validators.UserValidators
{
    public class UserForRegistrationDtoValidator : AbstractValidator<UserForRegistrationDto>    {
        public UserForRegistrationDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty()
                                    .MaximumLength(128);
            RuleFor(x => x.Email).NotEmpty()
                                 .MaximumLength(128);
            RuleFor(x => x.Password).NotEmpty()
                                    .MinimumLength(5)
                                    .MaximumLength(30);
            RuleFor(x => x.PhoneNumber).Matches(@"^$|(^[+])(\d{2,3})(\d{2,3})(\d{7})");
        }
    }

    public class AuthenticationCredentialsDtoValidator : AbstractValidator<AuthenticationCredentialsDto> 
    {
        public AuthenticationCredentialsDtoValidator() 
        {
            RuleFor(x => x.Email).NotEmpty()
                                 .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);
            RuleFor(x => x.Password).NotEmpty()
                                    .MinimumLength(5)
                                    .MaximumLength(30);
                                    //.Matches(@"(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{5,}");
        }
    }
}
