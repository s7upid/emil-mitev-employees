using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces;

public interface IAppDbContext
{
    DbSet<Employee> Employees { get; }

    DbSet<Project> Projects { get; }

    DbSet<EmployeeProject> EmployeeProjects { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
