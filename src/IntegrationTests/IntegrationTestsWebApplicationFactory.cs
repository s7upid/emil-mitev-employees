using IntegrationTests.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;
using Xunit;

namespace IntegrationTests;

public class IntegrationTestsWebApplicationFactory()
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new Lazy<MsSqlContainer>(BuildMsSqlContainer, LazyThreadSafetyMode.ExecutionAndPublication).Value;

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        // Retry connection to DB until successful
        var connectionString = _msSqlContainer.GetConnectionString();

        var isDbReady = false;
        var retries = 10;
        var delay = TimeSpan.FromSeconds(2);

        for (int i = 0; i < retries && !isDbReady; i++)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                isDbReady = true;
            }
            catch (Exception)
            {
                await Task.Delay(delay);
            }
        }

        if (!isDbReady)
        {
            throw new Exception("SQL Server Testcontainer did not become ready in time.");
        }
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Integration");
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Integration");
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var connectionString = _msSqlContainer.GetConnectionString();
            var overrideConfig = new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = connectionString
            };
            configBuilder.AddInMemoryCollection(overrideConfig);
        });

        base.ConfigureWebHost(builder);
    }

    private static MsSqlContainer BuildMsSqlContainer()
    {
        return new MsSqlBuilder()
            .WithImage(TestConstants.MsSqlImage)
            .WithPassword(TestConstants.MsSqlPassword)
            .Build();
    }
}
