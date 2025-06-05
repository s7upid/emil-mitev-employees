using Application.DTOs.Response;
using Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Services;

public interface IEmployeeProjectService
{
    Task<ValueResult<string>> ProcessEmployeeProjectFileAsync(IFormFile file, CancellationToken cancellationToken);
    Task<ValueResult<List<EmployeePairDto>>> GetAllEmployeePairsSortedAsync(CancellationToken cancellation);
}
