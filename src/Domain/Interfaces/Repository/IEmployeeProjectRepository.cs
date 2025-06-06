using Domain.Entities;

namespace Domain.Interfaces.Repository;

public interface IEmployeeProjectRepository
{
    Task AddRangeAsync(IEnumerable<EmployeeProject> records, CancellationToken cancellationToken);

    Task<IList<EmployeeProject>> GetAllAsync(CancellationToken cancellationToken);
}