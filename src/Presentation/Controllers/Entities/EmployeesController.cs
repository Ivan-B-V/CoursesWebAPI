using CoursesWebAPI.Application.Contracts.EntityServices;
using CoursesWebAPI.Core.Common.Enums;
using CoursesWebAPI.Shared.Authorization;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Helpers;
using CoursesWebAPI.Shared.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers.Entities;

[Route("api/employees")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class EmployeesController : ApiControllerBase
{
    private readonly IEmployeeService employeeService;

    public EmployeesController(ILogger logger, IEmployeeService employeeService) : base(logger)
    {
        this.employeeService = employeeService;
    }

    [HasPermission(Permissions.ViewEmployees)]
    [HttpGet]
    public async Task<IActionResult> GetEmployeesByParameters([FromQuery] PersonQueryParameters parameters, CancellationToken cancellationToken)
    {
        var pageList = await employeeService.GetEmployeesAsync(parameters, cancellationToken);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageList.MetaData));
        return Ok(pageList.Items);
    }

    //[HasPermission(Permissions.ViewEmployees)]
    [HttpGet("{id:guid}", Name = nameof(GetEmployeeById))]
    public async Task<IActionResult> GetEmployeeById(Guid id, CancellationToken cancellationToken)
    {
        var employee = await employeeService.GetByIdAsync(id, cancellationToken);
        return Ok(employee);
    }

    //[HasPermission(Permissions.ManageEmployees)]
    [HttpPost(Name = nameof(CreateEmployee))]
    public async Task<IActionResult> CreateEmployee(EmployeeForCreatingDto employeeForCreatingDto, CancellationToken cancellationToken)
    {
        var result = await employeeService.CreateAsync(employeeForCreatingDto, cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtRoute(nameof(GetEmployeeById), new { id = result.Value.Id }, result.Value);
        }
        return BadRequest(result.ToResultDto());
    }

    [HasPermission(Permissions.ManageEmployees)]
    [HttpPut("{id:guid}", Name = nameof(UpdateEmployee))]
    public async Task<IActionResult> UpdateEmployee(Guid id, EmployeeForUpdateDto employeeForUpdateDto, CancellationToken cancellationToken)
    {
        var result = await employeeService.UpdateAsync(id, employeeForUpdateDto, cancellationToken);
        if (result.IsSuccess) 
        {
            return NoContent();
        }
        return BadRequest(result.ToResultDto());
    }

    [HasPermission(Permissions.ManageEmployees)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid id, CancellationToken cancellationToken)
    {
        await employeeService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
