using Domain.Interfaces;
using Domain.Interfaces.Repository;

namespace Infrastructure.Data;

public class UnitOfWork(IAppDbContext context, IEmployeeProjectRepository employeeRepo) : IUnitOfWork
{
    private readonly IAppDbContext _context = context;

    public IEmployeeProjectRepository EmployeeProjects { get; } = employeeRepo;

    public async Task CompleteAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
