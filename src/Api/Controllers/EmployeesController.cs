using Application.DTOs.Request;
using Application.DTOs.Response;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(IEmployeeProjectService employeeProjectService) : ControllerBase
{
    private readonly IEmployeeProjectService _employeeProjectService = employeeProjectService;

    [HttpPost]
    [Route("upload")]
    [SwaggerOperation(Summary = "Add relations between employees and projects.")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload([FromForm] FileUploadRequest request, CancellationToken cancellationToken)
    {
        if (request == null || request.File == null || request.File.Length == 0)
        {
            return BadRequest("No file provided");
        }

        var result = await _employeeProjectService.ProcessEmployeeProjectFileAsync(request.File, cancellationToken);

        if (result.IsFailed)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    [Route("top-pair")]
    [SwaggerOperation(Summary = "Returns top pairs of employees.")]
    [ProducesResponseType(typeof(IList<EmployeePairDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTopWorkedTogetherPair(CancellationToken cancellationToken)
    {
        var result = await _employeeProjectService.GetAllEmployeePairsSortedAsync(cancellationToken);

        if (result.IsFailed)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Value);
    }
}