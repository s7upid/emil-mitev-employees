using Application.DTOs.Request;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(IEmployeeProjectService employeeProjectService) : ControllerBase
{
    private readonly IEmployeeProjectService _employeeProjectService = employeeProjectService;

    [HttpPost("upload")]
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

    [HttpGet("longest-pair")]
    public async Task<IActionResult> GetLongestWorkedTogetherPair(CancellationToken cancellationToken)
    {
        var result = await _employeeProjectService.GetAllEmployeePairsSortedAsync(cancellationToken);

        if (result.IsFailed)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Value);
    }
}