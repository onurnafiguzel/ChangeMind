namespace ChangeMind.IntegrationTests.Fixtures;

using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;

// IAsyncLifetime: xUnit'in test class başında/sonunda async setup/teardown hook'u.
// Tüm test sınıfları bu fixture'ı IClassFixture<PostgreSqlFixture> ile paylaşır.
// Container tek bir kez başlar, tüm testler biter, sonra durur.
public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithDatabase("changemind_test")
        .WithUsername("changemind")
        .WithPassword("changemind_test_pass")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .Build();

    public string ConnectionString => _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
