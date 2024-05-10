using CoursesWebAPI.Application.Contracts.EntityServices;
using CoursesWebAPI.Core.Common.Enums;
using CoursesWebAPI.Shared.Authorization;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Helpers;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers.Entities;

[Route("api/activitytypes")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class ActivityTypesController : ApiControllerBase
{
    private readonly IActivityTypeService activityTypeService;

    public ActivityTypesController(ILogger logger, IActivityTypeService activityTypeService) : base(logger)
    {
        this.activityTypeService = activityTypeService;
    }

    //[HasPermission(Permissions.ViewActivityTypes)]
    [HttpGet(Name = nameof(GetActivityTypes))]
    public async Task<IActionResult> GetActivityTypes(CancellationToken cancellationToken)
    {
        var activityTypes = await activityTypeService.GetAllAsync(cancellationToken);
        return Ok(activityTypes);
    }

    //[HasPermission(Permissions.ManageActivityTypes)]
    [HttpGet("{id:guid}", Name = nameof(GetActivityTypeById))]
    public async Task<IActionResult> GetActivityTypeById([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var activityType = await activityTypeService.GetByIdAsync(id, cancellationToken);
        return Ok(activityType);
    }

    //[HasPermission(Permissions.ManageActivityTypes)]
    [HttpPost(Name = nameof(CreateActivitytType))]
    public async Task<IActionResult> CreateActivitytType([FromBody] ActivityTypeForCreatingDto activityTypeDto, CancellationToken cancellationToken)
    {
        var result = await activityTypeService.CreateAsync(activityTypeDto, cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtRoute(nameof(GetActivityTypeById), new { id = result.Value.Id }, result.Value);
        }
        return BadRequest(result.ToResultDto());
    }

    [HasPermission(Permissions.ManageActivityTypes)]
    [HttpPut("{id:guid}", Name = nameof(UpdateActivityType))]
    public async Task<IActionResult> UpdateActivityType(Guid id, [FromBody] ActivityTypeForUpdateDto activityTypeDto, CancellationToken cancellationToken)
    {
        await activityTypeService.UpdateAsync(id, activityTypeDto, cancellationToken);
        return NoContent();
    }

    [HasPermission(Permissions.ManageActivityTypes)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteActivityType(Guid id, CancellationToken cancellationToken)
    {
        if(!await activityTypeService.DeleteAsync(id, cancellationToken))
        {
            return BadRequest();
        }
        return NoContent();
    }
}
