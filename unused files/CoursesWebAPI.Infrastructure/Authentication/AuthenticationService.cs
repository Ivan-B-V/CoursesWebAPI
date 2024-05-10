using CoursesWebAPI.Application.Contracts.IdentityServices;
using CoursesWebAPI.Core.Identity;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace CoursesWebAPI.Infrastructure.Authentication;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly ITokenService tokenService;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;
    private readonly ILogger logger;

    public AuthenticationService(UserManager<User> userManager,
        RoleManager<Role> roleManager,
        TokenValidationParameters tokenValidationParameters,
        ITokenService tokenService,
        ILogger logger)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.logger = logger;
        this.tokenService = tokenService;
    }

    public async Task<Result<AccessRefreshTokenDto>> AuthenticateAsync(AuthenticationCredentialsDto authenticationCredentials, CancellationToken cancellationToken)
    {
        if (authenticationCredentials == null ||
            authenticationCredentials.Email is null ||
            authenticationCredentials.Password is null)
        {
            return Result.Fail("Invalid login credentials.");
        }
        var user = await userManager.FindByEmailAsync(authenticationCredentials.Email);
        if (user is null)
        {
            return Result.Fail($"There is no user with email: {authenticationCredentials.Email}.");
        }
        var result = await CheckIfUserCanBeAuthenticatedAsync(user, authenticationCredentials, cancellationToken);
        if (result.IsFailed)
        {
            return result.ToResult<AccessRefreshTokenDto>();
        }
        var tokens = await tokenService.CreateTokensAsync(user, cancellationToken);
        return tokens;
    }

    public async Task<Result> BlockUserAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Fail("Invalid credentials.");
        }
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Fail($"There is no user with email: {email}.");
        }
        await userManager.UpdateAsync(user);
        return Result.Ok();
    }

    public async Task<Result> LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        //refactor this method
        string jwt = string.Empty;
        //if (contextAccessor.HttpContext is not null)
        //{
        //    jwt = await contextAccessor.HttpContext.GetTokenAsync("access_token") ??
        //        throw new NullReferenceException("There is no access token in context.");
        //}
        return await tokenService.RemoveRefreshTokenForJwtAsync(jwt, cancellationToken);
    }

    public async Task<Result> CanUserBeAuthenticatedAsync(AuthenticationCredentialsDto authenticationCredentials, CancellationToken cancellationToken = default)
    {
        if (authenticationCredentials == null ||
            authenticationCredentials.Email is null ||
            authenticationCredentials.Password is null)
        {
            return Result.Fail("Invalid login credentials.");
        }
        var user = await userManager.FindByEmailAsync(authenticationCredentials.Email);
        if (user is null)
        {
            return Result.Fail($"There is no user with email: {authenticationCredentials.Email}.");
        }
        var result = await CheckIfUserCanBeAuthenticatedAsync(user, authenticationCredentials, cancellationToken);
        if (result.IsSuccess)
        {
            return Result.Ok();
        }
        return Result.Fail("User cannot be authenticated.");
    }

    private async Task<Result> CheckIfUserCanBeAuthenticatedAsync(User user, AuthenticationCredentialsDto authenticationCredentials, CancellationToken cancellationToken = default)
    {
        if (!user.EmailConfirmed)
        {
            return Result.Fail("Email wasn't confirmed.");
        }
        var authResult = await userManager.CheckPasswordAsync(user, authenticationCredentials.Password);
        if (authResult)
        {
            return Result.Ok();
        }
        return Result.Fail("Invalid login credentials.");
    }
}

