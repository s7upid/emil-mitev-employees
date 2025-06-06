using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class EmployeeProjectRepository(IAppDbContext context) : IEmployeeProjectRepository
{
    private readonly IAppDbContext _context = context;

    public async Task AddRangeAsync(IEnumerable<EmployeeProject> records, CancellationToken cancellationToken)
    {
        var existingEmployeeIds = _context.Employees.Select(e => e.Id).ToHashSet();
        var existingProjectIds = _context.Projects.Select(p => p.Id).ToHashSet();

        foreach (var record in records)
        {
            if (!existingEmployeeIds.Contains(record.EmployeeId))
            {
                _context.Employees.Add(new Employee { Id = record.EmployeeId });
                existingEmployeeIds.Add(record.EmployeeId);
            }

            if (!existingProjectIds.Contains(record.ProjectId))
            {
                _context.Projects.Add(new Project { Id = record.ProjectId });
                existingProjectIds.Add(record.ProjectId);
            }
        }

        await _context.EmployeeProjects.AddRangeAsync(records, cancellationToken);
    }

    public async Task<IList<EmployeeProject>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.EmployeeProjects
            .Include(ep => ep.Employee)
            .Include(ep => ep.Project)
            .ToListAsync(cancellationToken);
    }
}
