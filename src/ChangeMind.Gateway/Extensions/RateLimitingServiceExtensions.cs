namespace ChangeMind.Gateway.Extensions;

using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class RateLimitingServiceExtensions
{
    public static IServiceCollection AddGatewayRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var strict   = configuration.GetSection("RateLimiting:Strict");
        var pub      = configuration.GetSection("RateLimiting:Public");
        var standard = configuration.GetSection("RateLimiting:Standard");
        var admin    = configuration.GetSection("RateLimiting:Admin");
        var payment  = configuration.GetSection("RateLimiting:Payment");

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.Headers.RetryAfter =
                    context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
                        ? ((int)retryAfter.TotalSeconds).ToString()
                        : "60";

                await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", token);
            };

            // Strict — Login, change-password: per-IP brute force koruması (auth öncesi, userId yok)
            options.AddPolicy<string>("strict", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit          = strict.GetValue("PermitLimit", 5),
                        Window               = TimeSpan.FromSeconds(strict.GetValue("WindowSeconds", 60)),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit           = 0
                    }));

            // Public — Register/signup: per-IP spam koruması (auth öncesi, userId yok)
            options.AddPolicy<string>("public", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit          = pub.GetValue("PermitLimit", 20),
                        Window               = TimeSpan.FromSeconds(pub.GetValue("WindowSeconds", 60)),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit           = 0
                    }));

            // Standard — Authenticated genel endpoint'ler: per-user sliding window
            options.AddPolicy<string>("standard", context =>
            {
                var userId = context.User.FindFirst("sub")?.Value
                          ?? context.Connection.RemoteIpAddress?.ToString()
                          ?? "anonymous";

                return RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: $"standard:{userId}",
                    factory: _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit          = standard.GetValue("PermitLimit", 60),
                        Window               = TimeSpan.FromSeconds(standard.GetValue("WindowSeconds", 60)),
                        SegmentsPerWindow    = 6,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit           = 0
                    });
            });

            // Admin — Admin işlemleri: per-user token bucket (burst'a izin verir)
            options.AddPolicy<string>("admin", context =>
            {
                var userId = context.User.FindFirst("sub")?.Value
                          ?? context.Connection.RemoteIpAddress?.ToString()
                          ?? "anonymous";

                return RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: $"admin:{userId}",
                    factory: _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit           = admin.GetValue("TokenLimit", 30),
                        ReplenishmentPeriod  = TimeSpan.FromSeconds(admin.GetValue("ReplenishmentPeriodSeconds", 10)),
                        TokensPerPeriod      = admin.GetValue("TokensPerPeriod", 10),
                        AutoReplenishment    = true,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit           = 0
                    });
            });

            // Payment — per-user eş zamanlı slot sınırı (bulkhead): her kullanıcı kendi concurrency limitine sahip
            options.AddPolicy<string>("payment", context =>
            {
                var userId = context.User.FindFirst("sub")?.Value
                          ?? context.Connection.RemoteIpAddress?.ToString()
                          ?? "anonymous";

                return RateLimitPartition.GetConcurrencyLimiter(
                    partitionKey: $"payment:{userId}",
                    factory: _ => new ConcurrencyLimiterOptions
                    {
                        PermitLimit          = payment.GetValue("MaxConcurrent", 5),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit           = payment.GetValue("QueueLimit", 2)
                    });
            });
        });

        return services;
    }
}
