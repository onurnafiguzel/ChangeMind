namespace ChangeMind.IntegrationTests.Fixtures;

using ChangeMind.Application.Services;
using ChangeMind.Application.UseCases.Payments.Commands;
using ChangeMind.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// WebApplicationFactory<Program>: Gerçek ASP.NET Core pipeline'ını in-process başlatır.
// ConfigureServices ile DI container'ı doğrudan değiştiriyoruz — config override'a gerek yok.
public sealed class WebAppFactory(PostgreSqlFixture db) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // "Testing" environment → DataSeeder çalışmaz (Program.cs IsDevelopment() kontrol eder)
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // DbContext'i TestContainers connection string'iyle replace et.
            // ConfigureAppConfiguration yerine bunu kullanıyoruz çünkü .NET 10'da
            // WebApplicationFactory'de config override'lar DbContext kayıt sırasında uygulanmaz.
            services.RemoveAll<DbContextOptions<ChangeMindDbContext>>();
            services.RemoveAll<ChangeMindDbContext>();

            services.AddDbContext<ChangeMindDbContext>(options =>
                options.UseNpgsql(
                    db.ConnectionString,
                    npgsql => npgsql.MigrationsAssembly("ChangeMind.Infrastructure")));

            // Redis → no-op NullCacheService (fail-open davranışını test ortamında garantiler)
            services.RemoveAll<ICacheService>();
            services.AddSingleton<ICacheService, NullCacheService>();

            // Idempotency → no-op (her istek yeni sayılır)
            services.RemoveAll<IIdempotencyService>();
            services.AddSingleton<IIdempotencyService, NullIdempotencyService>();
        });
    }
}

file sealed class NullCacheService : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        => Task.FromResult(default(T?));

    public Task SetAsync<T>(string key, T value, TimeSpan? ttl = null, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}

file sealed class NullIdempotencyService : IIdempotencyService
{
    public Task<IdempotencyCheckResult> CheckAsync(Guid userId, Guid idempotencyKey, CancellationToken cancellationToken = default)
        => Task.FromResult(new IdempotencyCheckResult(IdempotencyStatus.New, null));

    public Task CommitAsync(Guid userId, Guid idempotencyKey, PaymentProcessResponse response, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
