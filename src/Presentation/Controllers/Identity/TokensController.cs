using CoursesWebAPI.Application.Contracts.IdentityServices;
using CoursesWebAPI.Shared.DataTransferObjects.Helpers;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers.Identity;

[Route("api/tokens")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class TokensController : ApiControllerBase
{
    private readonly ITokenService tokenService;

    public TokensController(ILogger logger, ITokenService tokenService) : base(logger)
    {
        this.tokenService = tokenService;
    }

    [HttpPost("refresh", Name = nameof(RefreshTokens))]
    public async Task<IActionResult> RefreshTokens(TokensForRefreshDto accessRefreshTokenDto, CancellationToken cancellationToken)
    {
        var result = await tokenService.RefreshTokensAsync(accessRefreshTokenDto, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.ToResultDto());
        }
        return BadRequest(result.ToResultDto());
    }
}
