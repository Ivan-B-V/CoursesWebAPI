using CoursesWebAPI.Application.Contracts.IdentityServices;
using CoursesWebAPI.Core.Common.Enums;
using CoursesWebAPI.Shared.Authorization;
using CoursesWebAPI.Shared.DataTransferObjects.Helpers;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using CoursesWebAPI.Shared.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers.Identity
{
    [Route("api/users")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public sealed class UserManagementController : ApiControllerBase
    {
        private readonly IUserManagementService userManagementService;

        public UserManagementController(ILogger logger,
            IUserManagementService userManagementService)
            : base(logger)
        {
            this.userManagementService = userManagementService;
        }

        [HasPermission(Permissions.ViewUsers)]
        [HttpGet(Name = nameof(GetUsersByParameters))]
        public async Task<IActionResult> GetUsersByParameters([FromQuery] UserQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            var result = await userManagementService.GetUsersByParametersAsync(queryParameters, cancellationToken);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.MetaData));
            return Ok(result.Items);
        }

        //[HasPermission(Permissions.ManageUsers)]
        [HttpPost(Name = nameof(RegisterUser))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistrationDto, CancellationToken cancellationToken)
        {
            var result = await userManagementService.RegisterUserAsync(userForRegistrationDto, cancellationToken);
            if (result.IsSuccess)
            {
                return Ok(result.ToResultDto());
            }
            return BadRequest(result.ToResultDto());
        }

        [HasPermission(Permissions.ManageUsers)]
        [HttpPut("{id:guid}", Name = nameof(UpdateUser))]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserForUpdateDto userForUpdateDto, CancellationToken cancellationToken)
        {
            var result = await userManagementService.UpdateUserAsync(id, userForUpdateDto, cancellationToken);
            if (result.IsSuccess) 
            {
                return NoContent();
            }
            return BadRequest(result.ToResultDto());
        }
    }
}
