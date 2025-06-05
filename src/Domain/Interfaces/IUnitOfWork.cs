using Domain.Interfaces.Repository;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IEmployeeProjectRepository EmployeeProjects { get; }

    Task CompleteAsync(CancellationToken cancellationToken);
}
