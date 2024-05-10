using CoursesWebAPI.Application.Contracts.IdentityServices;
using CoursesWebAPI.Core.Identity;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Security.Claims;

namespace CoursesWebAPI.Infrastructure.Authentication;

public sealed class AuthorizationService : IApplicationAuthorizationService
{
    private readonly ILogger logger;
    private readonly UserManager<User> userManager;
    private readonly ITokenService tokenService;

    public AuthorizationService(ILogger logger, UserManager<User> userManager,
        ITokenService tokenService)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.tokenService = tokenService;
    }

    public Result<ClaimsPrincipal> ValidateJwt(string jwt)
    {
        var result = tokenService.ValidateJWT(jwt);
        return result;
    }

    public async Task<Result> AuthorizeUserByJwtAsync(string jwt, string method, string endPoint, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(method));
        ArgumentException.ThrowIfNullOrEmpty(nameof(endPoint));

        var result = tokenService.ValidateJWT(jwt);
        if (result.IsFailed)
        {
            return result.ToResult();
        }
        var userId = result.Value.Claims.First(c => c.Type.Equals("id")).Value;
        var user = await userManager.FindByIdAsync(userId);

        return result.ToResult();
    }

    public async Task<Result> AuthorizeUserByIdAsync(Guid id, string method, string endPoint, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(method));
        ArgumentException.ThrowIfNullOrEmpty(nameof(endPoint));

        return Result.Ok();
    }

    public async Task<Result> AuthorizeUserByIdAsync(Guid id, string actionName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}