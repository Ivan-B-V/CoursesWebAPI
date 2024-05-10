using CoursesWebAPI.Core.Common;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using CoursesWebAPI.Shared.RequestFeatures;
using FluentResults;

namespace CoursesWebAPI.Application.Contracts.IdentityServices
{
    public interface IUserManagementService
    {
        public Task<Result> RegisterUserAsync(UserForRegistrationDto userForRegistrationDto, CancellationToken cancellationToken = default);
        public Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
        public Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        public Task<PageList<UserDto>> GetUsersByParametersAsync(UserQueryParameters parameters, CancellationToken cancellationToken = default);
        public Task<Result> BindUserWithPersonAsync(string email, Guid personId, CancellationToken cancellationToken = default);
        public Task<Result> UpdateUserAsync(Guid userId, UserForUpdateDto userForUpdateDto, CancellationToken cancellationToken = default);
        public Task<Result> UpdateUserRolesAsync(Guid userId, IEnumerable<string> roles, CancellationToken cancellationToken = default);
        public Task<Result> AddUserToRolesAsync(Guid userId, IEnumerable<string> roleNames, CancellationToken cancellationToken = default);
        public Task<Result> SendConfiramtionEmailAsync(string email, CancellationToken cancellationToken = default);
        public Task<Result> ConfirmEmailAsync(string email, string token, CancellationToken cancellationToken = default);
        public Task<Result> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken = default);
        public Task<Result<string>> GetEmailConfirmationTokenAsync(string email, CancellationToken cancellationToken = default);
    }
}
