using CoursesWebAPI.Application.Contracts.IdentityServices;
using CoursesWebAPI.Shared.DataTransferObjects.Helpers;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers.Identity;

[Route("api/authentication")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class AuthenticationController : ApiControllerBase
{
    private readonly IAuthenticationService authenticationService;
    private readonly IUserManagementService userManagementService;

    public AuthenticationController(ILogger logger,
        IAuthenticationService authenticationService, 
        IUserManagementService userManagementService) : base(logger)
    {
        this.authenticationService = authenticationService;
        this.userManagementService = userManagementService;
    }

    [HttpPost]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticationCredentialsDto authenticationCredentials, CancellationToken cancellationToken)
    {
        var result = await authenticationService.AuthenticateAsync(authenticationCredentials, cancellationToken);
        if(result.IsSuccess)
        {
            return Ok(result.ToResultDto());
        }
        return BadRequest(result.ToResultDto());
    }

    [HttpGet("~/api/confirmemail")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token, CancellationToken cancellationToken)
    {
        if (email is null || token is null)
        {
            return BadRequest("Invalid query parameters.");
        }
        var result = await userManagementService.ConfirmEmailAsync(email, Encoding.UTF8.GetString(Convert.FromBase64String(token)), cancellationToken);
        if (result.IsFailed)
        {
            return BadRequest(result.ToResultDto());
        }
        return Ok(result.ToResultDto());
    }

    [HttpGet("~/api/resetpassword", Name = nameof(GetResetPasswordLink))]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> GetResetPasswordLink([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Invalid email.");
        }
        var result = await authenticationService.GetResetPasswordLink(email);
        if (result.IsFailed)
        {
            return BadRequest(result.ToResultDto());
        }
        return Ok(); 
    }

    [HttpPost("~/api/resetpassword", Name = nameof(ResetPassword))]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> ResetPassword([FromQuery]string email, [FromQuery] string token, [FromBody] string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Invalid email.");
        }
        var result = await authenticationService.ResetPassword(email, token, password, cancellationToken);
        if (result.IsFailed)
        {
            return BadRequest(result.ToResultDto());
        }

        return Ok();
    }
}
