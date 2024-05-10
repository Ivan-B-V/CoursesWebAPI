using CoursesWebAPI.Application.Contracts.EntityServices;
using CoursesWebAPI.Core.Common.Enums;
using CoursesWebAPI.Shared.Authorization;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers.Entities
{
    [Route("api/students")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public sealed class StudentsController : ApiControllerBase
    {
        private readonly IStudentService studentService;

        public StudentsController(ILogger logger, IStudentService studentService) : base(logger)
        {
            this.studentService = studentService;
        }

        [HasPermission(Permissions.ViewStudents)]
        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] EmployeeQueryParameters queryParameters, CancellationToken cancellationToken)
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

    }
}
