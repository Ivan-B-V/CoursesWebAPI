using CoursesWebAPI.Application.Contracts.Data;
using CoursesWebAPI.Application.Contracts.IdentityServices;
using CoursesWebAPI.Infrastructure.ConfigurationModels;
using CoursesWebAPI.Core.Identity;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoursesWebAPI.Infrastructure.Authentication;

public sealed class TokenService : ITokenService
{
    private readonly ILogger logger;
    private readonly UserManager<User> userManager;
    private readonly IRepositoryManager repositoryManager;
    private readonly TokenValidationParameters jwtValidationParameters;
    private readonly JwtConfiguration jwtConfiguration;

    public TokenService(UserManager<User> userManager,
        ILogger logger, IRepositoryManager repositoryManager,
        IOptionsSnapshot<JwtConfiguration> configuration,
        TokenValidationParameters jwtValidationParameters)
    {
        this.userManager = userManager;
        this.logger = logger;
        this.repositoryManager = repositoryManager;
        this.jwtValidationParameters = jwtValidationParameters;
        jwtConfiguration = configuration.Value;
    }

    public async Task<Result<AccessRefreshTokenDto>> CreateTokensAsync(User user, CancellationToken cancellationToken = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenResult = await GenerateJwtAccessTokenAsync(user, cancellationToken);
        if (tokenResult.IsFailed)
        {
            return tokenResult.ToResult<AccessRefreshTokenDto>();
        }
        var rawJwt = tokenHandler.WriteToken(tokenResult.Value);

        var refreshTokenResult = await GenerateRefreshTokenAsync(new Guid(tokenResult.Value.Id), user.Id, cancellationToken);
        if (refreshTokenResult.IsFailed)
        {
            return refreshTokenResult.ToResult<AccessRefreshTokenDto>();
        }
        repositoryManager.RefreshTokenRepository.Create(refreshTokenResult.Value);
        return new AccessRefreshTokenDto(AccessToken: rawJwt, RefreshToken: refreshTokenResult.Value.Value);
    }

    public async Task<Result<AccessRefreshTokenDto>> RefreshTokensAsync(TokensForRefreshDto accessRefreshToken, CancellationToken cancellationToken = default)
    {
        if (accessRefreshToken is null)
        {
            return Result.Fail("Invalid tokens.");
        }

        var principalResult = GetPrincipalFromExpiredToken(accessRefreshToken.AccessToken);
        if (principalResult.IsFailed)
        {
            return principalResult.ToResult<AccessRefreshTokenDto>();
        }

        var jwtIdString = principalResult.Value.Claims.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Jti))?.Value;
        var userEmail = principalResult.Value.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
        if (!Guid.TryParse(jwtIdString, out var jwtId) 
            || string.IsNullOrEmpty(userEmail))
        {
            return Result.Fail("Invalid access token.");
        }

        var refreshToken = await repositoryManager.RefreshTokenRepository.GetByIdAsync(accessRefreshToken.RefreshToken, trackChanges: false, cancellationToken);
        if (refreshToken is null)
        {
            return Result.Fail("Invalid refresh token.");
        }

        //if (!refreshToken.Fingerprint.Equals(accessRefreshToken.Fingerprint))
        //{
        //    return Result.Fail("Invalid refresh token.");
        //}

        if (refreshToken.Expires < DateTime.UtcNow)
        {
            repositoryManager.RefreshTokenRepository.Delete(refreshToken.Id);
            return Result.Fail("Refresh token was expired.");
        }
        repositoryManager.RefreshTokenRepository.Delete(refreshToken.Id);

        var user = await userManager.FindByEmailAsync(userEmail);
        if (user is null)
        {
            return Result.Fail("Invalid user email.");
        }
        return await CreateTokensAsync(user, cancellationToken);
    }

    public Result<SecurityToken> GenerateJwtAccessToken(List<Claim> claims)
    {
        var tokenDescriptor = GenerateSecurityTokenDescriptor(claims);
        return new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
    }

    public async Task<Result<SecurityToken>> GenerateJwtAccessTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user is null)
            return Result.Fail("User is null.");

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.CreateToken(await GenerateSecurityTokenDescriptorAsync(user, cancellationToken));
    }

    public async Task<Result<RefreshToken>> GenerateRefreshTokenAsync(Guid jwtId, Guid userId, CancellationToken cancellationToken = default)
    {
        var newRefreshToken = new RefreshToken
        {
            JwtId = jwtId,
            UserId = userId,
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.Add(jwtConfiguration.RefreshTokenLifeTime)
        };
        repositoryManager.RefreshTokenRepository.Create(newRefreshToken);
        await repositoryManager.SaveChangesAsync(cancellationToken);
        return newRefreshToken;
    }

    public Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string jwtToken)
    {
        if (string.IsNullOrEmpty(jwtToken))
        {
            return Result.Fail("Invalid token.");
        }

        var expiredTokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = jwtValidationParameters.ValidateAudience,
            ValidateIssuer = jwtValidationParameters.ValidateIssuer,
            ValidateIssuerSigningKey = jwtValidationParameters.ValidateIssuerSigningKey,
            IssuerSigningKey = jwtValidationParameters.IssuerSigningKey,
            ValidateLifetime = false,
            ValidAudience = jwtValidationParameters.ValidAudience,
            ValidIssuer = jwtValidationParameters.ValidIssuer
        };

        var validationResult = ValidateJWTAndGetPrincipal(jwtToken, expiredTokenValidationParameters);
        return validationResult;
    }

    private Result<ClaimsPrincipal> ValidateJWTAndGetPrincipal(string jwtToken, TokenValidationParameters parameters)
    {
        try
        {
            ClaimsPrincipal claimsPrincipal;
            claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, parameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return Result.Fail("Invalid token or invalid algorithm signature.");
            }
            return claimsPrincipal;
        }
        catch (SecurityTokenValidationException exception)
        {
            logger.Information("JWT validation failed.\nException: {exception}", exception);
            return Result.Fail("Invalid token.");
        }
    }

    public Result<ClaimsPrincipal> ValidateJWT(string jwtToken)
    {
        if (string.IsNullOrEmpty(jwtToken))
        {
            return Result.Fail("Invalid token.");
        }
        return ValidateJWTAndGetPrincipal(jwtToken, jwtValidationParameters);
    }

    public async Task<Result<IEnumerable<Claim>>> GetUserClaimsAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user is null)
            return Result.Fail("User is null.");

        var claims = new List<Claim>();
        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(await userManager.GetClaimsAsync(user));
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        return claims;
    }

    public async Task<Result> RemoveRefreshTokenForJwtAsync(string jwt, CancellationToken cancellationToken = default)
    {
        var principalResult = GetPrincipalFromExpiredToken(jwt);
        if (principalResult.IsFailed)
        {
            return principalResult.ToResult();
        }
        var jwtId = principalResult.Value.Claims.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Jti))?.Value;
        if (jwtId is null)
        {
            return Result.Fail("Invalid token.");
        }
        var refreshToken = await repositoryManager.RefreshTokenRepository.GetByJwtIdAsync(new Guid(jwtId), trackChanges: false, cancellationToken);
        if (refreshToken is null)
        {
            return Result.Fail("Invalid token."); ;
        }
        repositoryManager.RefreshTokenRepository.Delete(refreshToken.Id);
        await repositoryManager.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }

    public SigningCredentials GetSigningCredentials() =>
        new(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

    private SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new(Encoding.UTF8.GetBytes(jwtConfiguration.Secret));

    private string GenerateRandomString(int length)
    {
        var source = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
        var generator = new Random();
        return new string(Enumerable.Repeat(source, length).Select(s => s[generator.Next(s.Length)]).ToArray());
    }
    private async Task<SecurityTokenDescriptor> GenerateSecurityTokenDescriptorAsync(User user, CancellationToken cancellationToken = default)
    {
        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToString())
            };
        var claimsResult = await GetUserClaimsAsync(user, cancellationToken);
        claims.AddRange(claimsResult.Value);
        return GenerateSecurityTokenDescriptor(claims);
    }

    private SecurityTokenDescriptor GenerateSecurityTokenDescriptor(IEnumerable<Claim>? claims)
    {
        return new SecurityTokenDescriptor
        {
            Issuer = jwtConfiguration.ValidIssuer,
            Audience = jwtConfiguration.ValidAudience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(jwtConfiguration.ExpiryTimeFrame),
            SigningCredentials = GetSigningCredentials()
        };
    }
}