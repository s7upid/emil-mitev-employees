using Domain.Entities;

namespace Domain.Interfaces.Repository;

public interface IEmployeeProjectRepository
{
    Task AddRangeAsync(IEnumerable<EmployeeProject> records, CancellationToken cancellationToken);

    Task<List<EmployeeProject>> GetAllAsync(CancellationToken cancellationToken);
}