using CoursesWebAPI.Application.Contracts.EntityServices;
using CoursesWebAPI.Core.Common.Enums;
using CoursesWebAPI.Shared.Authorization;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers.Entities;

[Route("api/students")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class StudentsController : ApiControllerBase
{
    private readonly IStudentService studentService;
    private readonly IActivityService activityService;

    public StudentsController(ILogger logger, IStudentService studentService, IActivityService activityService) : base(logger)
    {
        this.studentService = studentService;
        this.activityService = activityService;
    }

    [HasPermission(Permissions.ViewStudents)]
    [HttpGet]
    public async Task<IActionResult> GetStudents([FromQuery] PersonQueryParameters queryParameters, CancellationToken cancellationToken)
    {
        var students = await studentService.GetStudentsAsync(queryParameters, cancellationToken);
        return Ok(students);
    }

    [HasPermission(Permissions.ManageStudents)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStudentById(Guid id, CancellationToken cancellationToken)
    {
        var student = await studentService.GetByIdAsync(id, cancellationToken);
        return Ok(student);
    }

    [HasPermission(Permissions.ManageStudents)]
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] StudentForCreatingDto studentDto, CancellationToken cancellationToken)
    {
        var student = await studentService.CreateAsync(studentDto, cancellationToken);
        return CreatedAtRoute(nameof(GetStudentById), new { id = student.Id }, student);
    }

    [HasPermission(Permissions.ManageStudents)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] StudentForUpdateDto updatedStudentDto, CancellationToken cancellationToken)
    {
        await studentService.UpdateAsync(updatedStudentDto, cancellationToken);
        return NoContent();
    }

    [HasPermission(Permissions.ManageStudents)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteStudent(Guid id, CancellationToken cancellationToken)
    {
        await studentService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpGet("{id:guid}/activities")]
    public async Task<IActionResult> GetStudentSelfActivities([FromQuery] ActivityQueryParameters queryParameters, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("Unknown user.");
        }
        var student = await studentService.GetByUserIdAsync(new Guid(userId), cancellationToken);
        if(student is null)
        {
            return BadRequest("There is no such student.");
        }
        //get studentId or implement method to get student activities by user id
        var activities = await activityService.GetByStudentIdAsync(student.Id, cancellationToken);
        return Ok(activities);
    }
}
