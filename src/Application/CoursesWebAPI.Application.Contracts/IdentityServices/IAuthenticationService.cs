using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using FluentResults;

namespace CoursesWebAPI.Application.Contracts.IdentityServices
{
    public interface IAuthenticationService
    {
        public Task<Result<AccessRefreshTokenDto>> AuthenticateAsync(AuthenticationCredentialsDto authenticationCredentials, CancellationToken cancellationToken = default);
        public Task<Result> BlockUserAsync(string email, CancellationToken cancellationToken = default);
        public Task<Result> LogoutAsync(Guid userId, CancellationToken cancellationToken = default);
        public Task<Result> CanUserBeAuthenticatedAsync(AuthenticationCredentialsDto authenticationCredentials, CancellationToken cancellationToken = default);
        public Task<Result> ResetPassword(string email, string token, string newPassword, CancellationToken cancellationToken = default);
        public Task<Result> GetResetPasswordLink(string email, CancellationToken cancellationToken = default);
    }
}
