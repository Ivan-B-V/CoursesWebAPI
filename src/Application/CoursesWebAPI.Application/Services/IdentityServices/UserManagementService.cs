using CoursesWebAPI.Application.Contracts.Data;
using CoursesWebAPI.Application.Contracts.ExternalServices;
using CoursesWebAPI.Application.Contracts.IdentityServices;
using CoursesWebAPI.Application.Mappers;
using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Core.Entities;
using CoursesWebAPI.Core.Identity;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using CoursesWebAPI.Shared.Extensions;
using CoursesWebAPI.Shared.RequestFeatures;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CoursesWebAPI.Application.Services.IdentityServices
{
    public sealed class UserManagementService : IUserManagementService
    {
        private readonly ILogger logger;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IRepositoryManager repositoryManager;
        private readonly IEmailService emailService;

        public UserManagementService(ILogger logger,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IRepositoryManager repositoryManager,
            IEmailService emailService)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.repositoryManager = repositoryManager;
            this.emailService = emailService;
        }

        public async Task<Result> AddUserToRolesAsync(Guid userId, IEnumerable<string> roleNames, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                return Result.Fail("User not found.");
            }
            var identityResult = await userManager.AddToRolesAsync(user, roleNames);
            if (!identityResult.Succeeded)
            {
                return identityResult.ToResult();
            }
            return Result.Ok();
        }

        public async Task<Result> ConfirmEmailAsync(string email, string token, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Result.Fail("User not found.");
            }
            var identityResult = await userManager.ConfirmEmailAsync(user, token);
            if (!identityResult.Succeeded)
            {
                return identityResult.ToResult();
            }
            await userManager.RemoveFromRoleAsync(user, "Unconfirmed");
            logger.Information("Email: {email} was confirmed.", email);
            return Result.Ok();
        }

        public async Task<Result> BindUserWithPersonAsync(string email, Guid personId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Result.Fail("User not found.");
            }
            //refactor this 2 ifs
            if (await repositoryManager.StudentRepository.GetByIdAsync(personId, true, cancellationToken) is Person student)
            {
                student.UserId = user.Id;
            }
            if (await repositoryManager.EmployeeRepository.GetByIdAsync(personId, true, cancellationToken) is Person employee)
            {
                employee.UserId = user.Id;
            }
            await repositoryManager.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user?.ToUserDto();
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            return user?.ToUserDto();
        }

        public async Task<PageList<UserDto>> GetUsersByParametersAsync(UserQueryParameters parameters, CancellationToken cancellationToken = default)
        {
            var usersQuery = userManager.Users
                                        .Include(u => u.Roles)
                                        .AsSplitQuery(); //add sorting and filtering
            if (!string.IsNullOrEmpty(parameters.Role) && await roleManager.FindByNameAsync(parameters.Role) is Role role)
            {
                //var role = await roleManager.FindByNameAsync(parameters.Role);//.Roles.FirstOrDefaultAsync(r => r.Name.Equals(parameters.Role), cancellationToken: cancellationToken);
                usersQuery = usersQuery.Where(u => u.Roles.Contains(role));
            }
            var userPageList = await PageList<User>.ToPageListAsync(usersQuery, parameters.PageNumber, parameters.PageSize, cancellationToken);
            return PageList<UserDto>.ToPageList(userPageList.Items.ToUserDtos(), userPageList.MetaData);
        }

        public async Task<Result> RegisterUserAsync(UserForRegistrationDto userForRegistrationDto, CancellationToken cancellationToken = default)
        {
            var newUser = userForRegistrationDto.ToUser();
            var identityResult = await userManager.CreateAsync(newUser, userForRegistrationDto.Password);
            if (!identityResult.Succeeded)
            {
                logger.Warning("Cannot create user: {result}", identityResult.ToString());
                return identityResult.ToResult();
            }
            await repositoryManager.SaveChangesAsync(cancellationToken);
            logger.Information("User: {id}, {email} was created.", newUser.Id, newUser.Email);
            await SendConfiramtionEmailAsync(newUser.Email!, cancellationToken); //??? process the result
            return Result.Ok();
        }

        public async Task<Result> UpdateUserAsync(Guid userId, UserForUpdateDto userForUpdateDto, CancellationToken cancellationToken = default)
        {
            if (await userManager.FindByIdAsync(userId.ToString()) is not User dbUser)
            {
                return Result.Fail($"There is no user with id: {userId}");
            }
            UserMapper.Map(userForUpdateDto, dbUser);
            var updateResult = await userManager.UpdateAsync(dbUser);
            if (!updateResult.Succeeded) 
            {
                return updateResult.ToResult();
            }
            return Result.Ok();
        }

        public async Task<Result> UpdateUserRolesAsync(Guid userId, IEnumerable<string> roles, CancellationToken cancellationToken = default)
        {
            if (await userManager.FindByIdAsync(userId.ToString()) is not User user)
            {
                logger.Warning("User with id: {id} was not found.", userId);
                return Result.Fail($"There is no user with id: {userId}");
            }
            var removeFromRolesResult = await userManager.RemoveFromRolesAsync(user, roles);
            if (!removeFromRolesResult.Succeeded)
            {
                logger.Error("Remove user from roles error. {error}", removeFromRolesResult.ToString());
                return removeFromRolesResult.ToResult();
            }
            var addToRolesResult = await userManager.AddToRolesAsync(user, roles);
            if (!addToRolesResult.Succeeded)
            {
                logger.Error("Add user to roles error. {errors}", addToRolesResult.ToString());
                return addToRolesResult.ToResult();
            }
            return Result.Ok();
        }

        public async Task<Result<string>> GetEmailConfirmationTokenAsync(string email, CancellationToken cancellationToken = default)
        {
            if (await userManager.FindByEmailAsync(email) is not User user) 
            {
                logger.Warning("User with email: {email} was not found.", email);
                return Result.Fail("There is not such user.");
            }
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public async Task<Result> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken = default)
        {
            if (await userManager.FindByIdAsync(userId.ToString()) is not User user) 
            {
                logger.Warning("User with id: {id} was not found.", userId);
                return Result.Fail("There is no such user.");
            }
            var identityResult = await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            return identityResult.ToResult();
        }

        public async Task<Result> SendConfiramtionEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var tokenResult = await GetEmailConfirmationTokenAsync(email, cancellationToken);
            if (tokenResult.IsFailed)
            {
                logger.Warning("Failed to create confirmation token: {errors}", tokenResult.ToString());
                return tokenResult.ToResult();
            }
            var result = await emailService.SendVerificationEmailAsync(email, tokenResult.Value, cancellationToken);
            if (result.IsSuccess)
            {
                logger.Information("Confiramtion email was sent. Address: {email}", email);
            }
            else
            {
                logger.Warning("Confiramtion email wasn't sent. Address: {email}", email);
            }
            return result;
        }
    }
}
