using Catalog.API;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Marten;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Catalog.API.IntegrationTests.Fixtures;

/// <summary>
/// Boots the Catalog API against a disposable PostgreSQL instance so tests hit real infrastructure.
/// </summary>
public class CatalogApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;

    public CatalogApiFactory()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("catalogdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithImage("postgres:15-alpine")
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Run the API in a dedicated environment so we control configuration without touching production settings.
        builder.UseEnvironment("IntegrationTesting");

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            // Override only the database connection string to point to the disposable container.
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Database"] = _postgresContainer.GetConnectionString()
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
    }

    /// <summary>
    /// Ensures Marten schema exists and wipes documents so each test has an isolated store.
    /// </summary>
    public async Task ResetDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IDocumentStore>();

        // Apply schema changes in case the test database is brand new.
        await store.Storage.ApplyAllConfiguredChangesToDatabaseAsync();

        // Clear out any data left from previous tests.
        await store.Advanced.Clean.DeleteDocumentsByTypeAsync(typeof(Product));
    }
}
