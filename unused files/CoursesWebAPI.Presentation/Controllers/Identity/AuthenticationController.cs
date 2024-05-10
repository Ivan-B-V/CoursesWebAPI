using CoursesWebAPI.Application.Contracts.IdentityServices;
using CoursesWebAPI.Shared.DataTransferObjects.Helpers;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers.Identity
{
    [Route("api/authentication")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public sealed class AuthenticationController : ApiControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(ILogger logger,
            IAuthenticationService authenticationService) : base(logger)
        {
            this.authenticationService = authenticationService;
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
        [ProducesResponseType(typeof(IEnumerable<string>), 400)]
        public IActionResult ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            if (email is null || token is null)
            {
                return BadRequest("Invalid query parameters.");
            }
            //var result = await _service.UserManagmentService.ConfirmEmailAsync(email, Encoding.UTF8.GetString(Convert.FromBase64String(token)));
            return Ok();
        }
    }
}
