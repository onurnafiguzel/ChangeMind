namespace ChangeMind.Api.Extensions;

using System.Text.Json;
using ChangeMind.Application.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public static class HealthCheckServiceExtensions
{
    public static IServiceCollection AddHealthCheckServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var redisConnectionString = configuration
            .GetSection("Cache")
            .Get<CacheOptions>()?.ConnectionString ?? "localhost:6379";

        services.AddHealthChecks()
            .AddRedis(redisConnectionString,
                name: "redis",
                failureStatus: HealthStatus.Degraded,
                tags: ["cache", "infrastructure"])
            .AddNpgSql(
                configuration.GetConnectionString("DefaultConnection")!,
                name: "postgres",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["db", "infrastructure"]);

        return services;
    }

    public static IEndpointRouteBuilder MapHealthCheckEndpoints(this IEndpointRouteBuilder app)
    {
        // Tüm bağımlılıkların durumunu JSON olarak raporlar
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (ctx, report) =>
            {
                ctx.Response.ContentType = "application/json";
                var result = new
                {
                    status   = report.Status.ToString(),
                    duration = report.TotalDuration.TotalMilliseconds,
                    checks   = report.Entries.Select(e => new
                    {
                        name   = e.Key,
                        status = e.Value.Status.ToString(),
                        ms     = e.Value.Duration.TotalMilliseconds,
                        error  = e.Value.Exception?.Message
                    })
                };
                await ctx.Response.WriteAsync(
                    JsonSerializer.Serialize(result,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            }
        });

        // Kubernetes liveness probe — sadece process ayakta mı
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false
        });

        // Kubernetes readiness probe — tüm infrastructure servisleri hazır mı
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("infrastructure")
        });

        return app;
    }
}
