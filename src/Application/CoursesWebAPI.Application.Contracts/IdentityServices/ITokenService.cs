using CoursesWebAPI.Core.Identity;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using FluentResults;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CoursesWebAPI.Application.Contracts.IdentityServices
{
    public interface ITokenService
    {
        public Result<SecurityToken> GenerateJwtAccessToken(List<Claim> claims);
        public Task<Result<SecurityToken>> GenerateJwtAccessTokenAsync(User user, CancellationToken cancellationToken = default);
        public Task<Result<RefreshToken>> GenerateRefreshTokenAsync(Guid jwtId, Guid userId, CancellationToken cancellationToken = default);
        public Task<Result> RemoveRefreshTokenForJwtAsync(string jwt, CancellationToken cancellationToken = default);
        public Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string jwtToken);
        public Result<ClaimsPrincipal> ValidateJWT(string jwtToken);
        public Task<Result<IEnumerable<Claim>>> GetUserClaimsAsync(User user, CancellationToken cancellationToken = default);
        public Task<Result<AccessRefreshTokenDto>> CreateTokensAsync(User user, CancellationToken cancellationToken = default);
        public Task<Result<AccessRefreshTokenDto>> RefreshTokensAsync(TokensForRefreshDto accessRefreshToken, CancellationToken cancellationToken = default);
    }
}
