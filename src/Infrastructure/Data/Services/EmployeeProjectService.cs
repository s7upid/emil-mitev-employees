using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Domain.Common;
using Application.DTOs.Response;

namespace Infrastructure.Data.Services;

public class EmployeeProjectService(IUnitOfWork uow) : IEmployeeProjectService
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<ValueResult<string>> ProcessEmployeeProjectFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        try
        {
            var employeeProjects = new List<EmployeeProject>();

            using var reader = new StreamReader(file.OpenReadStream());
            var header = await reader.ReadLineAsync(cancellationToken);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue; // Skip empty or null lines
                }

                var columns = line.Split(',');

                if (columns.Length < 4)
                {
                    return ValueResult<string>.Failure("Invalid CSV format. Each row must have 4 columns.");
                }

                if (!int.TryParse(columns[0], out int empId) || !int.TryParse(columns[1], out int projectId))
                {
                    return ValueResult<string>.Failure("Invalid Employee or Project ID.");
                }

                var dateFrom = ParseDate(columns[2]);
                var dateTo = string.IsNullOrWhiteSpace(columns[3]) || columns[3].Trim().Equals("NULL", StringComparison.CurrentCultureIgnoreCase)
                    ? DateTime.Today
                    : ParseDate(columns[3]);

                employeeProjects.Add(new EmployeeProject
                {
                    EmployeeId = empId,
                    ProjectId = projectId,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                });
            }

            await _uow.EmployeeProjects.AddRangeAsync(employeeProjects, cancellationToken);
            await _uow.CompleteAsync(cancellationToken);

            return ValueResult<string>.Success("File uploaded and data stored successfully.");
        }
        catch (FormatException fe)
        {
            return ValueResult<string>.Failure($"Date format error: {fe.Message}");
        }
        catch (OperationCanceledException)
        {
            return ValueResult<string>.Failure("File processing was cancelled.");
        }
        catch (Exception ex)
        {
            return ValueResult<string>.Failure($"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<ValueResult<List<EmployeePairDto>>> GetAllEmployeePairsSortedAsync(CancellationToken cancellation)
    {
        try
        {
            var records = await _uow.EmployeeProjects.GetAllAsync(cancellation);

            // Group by pairs and calculate total days worked together
            var pairs = new Dictionary<(int, int), int>();

            foreach (var e1 in records)
            {
                foreach (var e2 in records)
                {
                    if (e1.EmployeeId >= e2.EmployeeId)
                    {
                        continue;
                    }

                    if (e1.ProjectId == e2.ProjectId)
                    {
                        var overlapStart = e1.DateFrom > e2.DateFrom ? e1.DateFrom : e2.DateFrom;
                        var overlapEnd = e1.DateTo < e2.DateTo ? e1.DateTo : e2.DateTo;

                        if (overlapEnd >= overlapStart)
                        {
                            var days = (overlapEnd - overlapStart).Days + 1;
                            var key = (e1.EmployeeId, e2.EmployeeId);

                            if (pairs.ContainsKey(key))
                            {
                                pairs[key] += days;
                            }
                            else
                            {
                                pairs[key] = days;
                            }
                        }
                    }
                }
            }

            var result = pairs.Select(p => new EmployeePairDto
            {
                EmployeeId1 = p.Key.Item1,
                EmployeeId2 = p.Key.Item2,
                TotalDaysWorked = p.Value
            })
            .OrderByDescending(x => x.TotalDaysWorked)
            .ToList();

            return ValueResult<List<EmployeePairDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return ValueResult<List<EmployeePairDto>>.Failure("Error calculating employee pairs: " + ex.Message);
        }
    }

    private DateTime ParseDate(string date)
    {
        string[] formats = ["yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy"];
        return DateTime.ParseExact(date.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
    }
}