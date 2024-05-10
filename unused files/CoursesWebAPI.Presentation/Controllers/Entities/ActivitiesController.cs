using CoursesWebAPI.Application.Contracts.EntityServices;
using CoursesWebAPI.Core.Common.Enums;
using CoursesWebAPI.Shared.Authorization;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers.Entities
{
    /// <summary>
    /// Activity controller
    /// </summary>
    [Route("api/activities")]
    [Consumes("application/json")]
    [Produces("application/json")]
    //[HasPermission("Permissions3|4")]
    //[HasPermission(Permissions.ViewActivities, Permissions.ManageActivities)]
    public sealed class ActivitiesController : ApiControllerBase
    {
        private readonly IActivityService activityService;

        public ActivitiesController(ILogger logger, IActivityService activityService) : base(logger)
        {
            this.activityService = activityService;
        }

        /// <summary>
        /// Get activities by query parameters
        /// </summary>
        /// <param name="queryParameters"><see cref="ActivityQueryParameters"/></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetActivities))]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActivityFullDto>))]
        public async Task<IActionResult> GetActivities([FromQuery] ActivityQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            var pageList = await activityService.GetByParametersAsync(queryParameters, cancellationToken);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageList.MetaData));
            return Ok(pageList.Items);
        }

        /// <summary>
        /// Get activity by id <see cref="Guid"/>
        /// </summary>
        /// <param name="id">Activity id <see cref="Guid"/></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns <see cref="ActivityFullDto"/></returns>
        [HttpGet("{id:guid}", Name = nameof(GetActivityById))]
        public async Task<IActionResult> GetActivityById(Guid id, CancellationToken cancellationToken)
        {
            var activity = await activityService.GetByIdAsync(id, cancellationToken);
            return Ok(activity);
        }

        /// <summary>
        /// Create new activity.
        /// </summary>
        /// <param name="activityDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Something</returns>
        /// <response code="201">Returns the newly created item.</response>
        /// <response code="400">Returns result with validation errors.</response>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ActivityFullDto))]
        [ProducesResponseType(400, Type = typeof(IEnumerable<string>))]
        public async Task<IActionResult> CreateActivity([FromBody] ActivityForCreatingDto activityDto, CancellationToken cancellationToken)
        {
            var result = await activityService.CreateAsync(activityDto, cancellationToken);
            if (result.IsFailed)
            {
                return BadRequest(result);
            }
            return CreatedAtRoute(nameof(GetActivityById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateActivity(Guid id, [FromBody] ActivityForUpdateDto activityDto, CancellationToken cancellationToken)
        {
            await activityService.UpdateAsync(id, activityDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteActivity(Guid id, CancellationToken cancellationToken)
        {
            await activityService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
