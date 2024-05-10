using FluentResults;
using System.Security.Claims;

namespace CoursesWebAPI.Application.Contracts.IdentityServices
{
    public interface IApplicationAuthorizationService
    {
        public Result<ClaimsPrincipal> ValidateJwt(string jwt);
        public Task<Result> AuthorizeUserByJwtAsync(string jwt, string method, string endPoint, CancellationToken cancellationToken = default);
        public Task<Result> AuthorizeUserByIdAsync(Guid id, string actionName, CancellationToken cancellationToken = default);
    }
}
