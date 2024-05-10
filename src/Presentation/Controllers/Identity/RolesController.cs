using CoursesWebAPI.Application.Contracts.IdentityServices;
using CoursesWebAPI.Core.Common.Enums;
using CoursesWebAPI.Shared.Authorization;
using CoursesWebAPI.Shared.DataTransferObjects.Helpers;
using CoursesWebAPI.Shared.DataTransferObjects.Identity;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers.Identity;

[Route("api/roles")]
[Produces("application/json")]
[Consumes("application/json")]
//[HasPermission(Permissions.Admin)]
public sealed class RolesController : ApiControllerBase
{
    private readonly IRoleService roleService;

    public RolesController(ILogger logger, IRoleService roleService) : base(logger)
    {
        this.roleService = roleService;
    }

    [HttpGet(Name = nameof(GetAllRoles))]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var roles = await roleService.GetAllRolesAsync(cancellationToken);
        return Ok(roles);
    }

    [HttpGet("{id:guid}", Name = nameof(GetRoleById))]
    public async Task<IActionResult> GetRoleById(Guid id, CancellationToken cancellationToken)
    {
        var role = await roleService.GetRoleByIdAsync(id, cancellationToken);
        return Ok(role);
    }

    [HttpGet("{name}", Name = nameof(GetRoleByName))]
    public async Task<IActionResult> GetRoleByName([FromRoute(Name = "name")] string name, CancellationToken cancellationToken)
    {
        var role = await roleService.GetRoleByNameAsync(name, cancellationToken);
        return Ok(role);
    }

    [HttpPost(Name = nameof(CreateRole))]
    public async Task<IActionResult> CreateRole([FromBody] RoleForCreatingDto roleForCreatingDto, CancellationToken cancellationToken)
    {
        var result = await roleService.CreateRoleAsync(roleForCreatingDto, cancellationToken);
        if (result.IsFailed)
        {
            return BadRequest(result.ToResultDto());
        }
        return CreatedAtRoute(nameof(GetRoleById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:guid}", Name = nameof(UpdateRole))]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] RoleForUpdateDto roleForUpdateDto, CancellationToken cancellationToken)
    {
        var result = await roleService.UpdateRole(id, roleForUpdateDto, cancellationToken);
        if (result.IsFailed)
        {
            return BadRequest(result.ToResultDto());
        }
        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = nameof(DeleteRole))]
    public async Task<IActionResult> DeleteRole(Guid id, CancellationToken cancellationToken)
    {
        await roleService.DeleteRoleAsync(id, cancellationToken);
        return NoContent();
    }
}
