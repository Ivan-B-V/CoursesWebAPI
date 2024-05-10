using CoursesWebAPI.Application.Contracts.EntityServices;
using CoursesWebAPI.Shared.DataTransferObjects.Entities;
using CoursesWebAPI.Shared.DataTransferObjects.Helpers;
using CoursesWebAPI.Shared.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace CoursesWebAPI.Presentation.Controllers.Entities;

[Route("api/contracts")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class ContractsController : ApiControllerBase
{
    private readonly IContractService contractService;

    public ContractsController(ILogger logger, IContractService contractService) : base(logger)
    {
        this.contractService = contractService;
    }

    [HttpGet(Name = nameof(GetContractsByParameters))]
    public async Task<IActionResult> GetContractsByParameters([FromQuery] ContractQueryParameters parameters, CancellationToken cancellationToken)
    {
        var pageList = await contractService.GetByParametersAsync(parameters, cancellationToken);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageList.MetaData));
        return Ok(pageList.Items);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetContractById(Guid id, CancellationToken cancellationToken)
    {
        var contract = await contractService.GetByIdAsync(id, cancellationToken);
        return Ok(contract);
    }

    [HttpPost(Name = nameof(CreateContract))]
    public async Task<IActionResult> CreateContract([FromBody] ContractForCreatingDto contractDto, CancellationToken cancellationToken)
    {
        var result = await contractService.CreateAsync(contractDto, cancellationToken);
        if (result.IsSuccess)
        {
            return CreatedAtRoute(nameof(GetContractById), new { id = result.Value.Id }, result.Value);
        }
        return BadRequest(result.ToResultDto());
    }

    [HttpPut("{id:guid}", Name = nameof(UpdateContract))]
    public async Task<IActionResult> UpdateContract(Guid id, [FromBody] ContractForUpdateDto contractDto, CancellationToken cancellationToken)
    {
        var result = await contractService.UpdateAsync(id, contractDto, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.ToResultDto());
    }

    [HttpDelete("{id:guid}", Name = nameof(DeleteContract))]
    public async Task<IActionResult> DeleteContract(Guid id, CancellationToken cancellationToken)
    {
        var result = await contractService.DeleteAsync(id, cancellationToken);
        if(result)
            return NoContent();
        return BadRequest();
    }
}
