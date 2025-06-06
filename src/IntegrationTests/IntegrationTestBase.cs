using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests;

public class IntegrationTestBase : IClassFixture<IntegrationTestsWebApplicationFactory>, IDisposable
{
    protected readonly IntegrationTestsWebApplicationFactory TestFactory;
    protected readonly Mock<HttpMessageHandler> FreedompayHandler = new();
    protected readonly HttpClient Client;
    protected readonly IDistributedCache Cache;
    protected readonly IAppDbContext DbContext;
    protected readonly IUnitOfWork UnitOfWork;

    public IntegrationTestBase(IntegrationTestsWebApplicationFactory factory)
    {
        TestFactory = factory;
        var scope = factory.Services.CreateScope();

        Client = factory.CreateClient();
        DbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        UnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        Initialize().GetAwaiter().GetResult();
    }

    protected async Task Initialize()
    {
        await ClearInfrastructure();
    }

    protected async Task ClearInfrastructure()
    {
        var concreteDbContext = (AppDbContext)DbContext;
        await concreteDbContext.Database.ExecuteSqlRawAsync("DELETE FROM Employees");
        await concreteDbContext.Database.ExecuteSqlRawAsync("DELETE FROM Projects");
        await concreteDbContext.Database.ExecuteSqlRawAsync("DELETE FROM EmployeeProjects");
        await UnitOfWork.CompleteAsync(default);
    }

    public void Dispose()
    {
        // Cleanup after test run if needed (e.g., release resources)
        // Dispose of objects like Client or Cache if they are disposable
        Client.Dispose();
    }
}
