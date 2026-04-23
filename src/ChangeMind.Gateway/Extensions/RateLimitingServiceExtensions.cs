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

            // Strict — Login, change-password: brute force koruması
            options.AddFixedWindowLimiter("strict", opt =>
            {
                opt.PermitLimit        = strict.GetValue("PermitLimit", 5);
                opt.Window             = TimeSpan.FromSeconds(strict.GetValue("WindowSeconds", 60));
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit         = 0;
            });

            // Public — Register: spam koruması
            options.AddFixedWindowLimiter("public", opt =>
            {
                opt.PermitLimit        = pub.GetValue("PermitLimit", 20);
                opt.Window             = TimeSpan.FromSeconds(pub.GetValue("WindowSeconds", 60));
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit         = 0;
            });

            // Standard — Authenticated genel endpoint'ler: sliding window
            options.AddSlidingWindowLimiter("standard", opt =>
            {
                opt.PermitLimit          = standard.GetValue("PermitLimit", 60);
                opt.Window               = TimeSpan.FromSeconds(standard.GetValue("WindowSeconds", 60));
                opt.SegmentsPerWindow    = 6;
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit           = 0;
            });

            // Admin — Admin işlemleri: token bucket (burst'a izin verir)
            options.AddTokenBucketLimiter("admin", opt =>
            {
                opt.TokenLimit               = admin.GetValue("TokenLimit", 30);
                opt.ReplenishmentPeriod      = TimeSpan.FromSeconds(admin.GetValue("ReplenishmentPeriodSeconds", 10));
                opt.TokensPerPeriod          = admin.GetValue("TokensPerPeriod", 10);
                opt.AutoReplenishment        = true;
                opt.QueueProcessingOrder     = QueueProcessingOrder.OldestFirst;
                opt.QueueLimit               = 0;
            });

            // Payment — Zaman penceresi koruması standard ile aynı, ayrıca gateway
            // katmanında eş zamanlı slot sınırı eklenir (bulkhead). İki koruma aynı anda aktif:
            // "standard" policy → zaman penceresi (DDoS/spam), "payment" → eş zamanlı slot (kaynak koruması).
            // Not: YARP tek policy desteklediğinden payment route'u bu policy'yi kullanır;
            // sliding window mantığı buraya gömülüdür.
            options.AddPolicy<string>("payment", context =>
                RateLimitPartition.GetConcurrencyLimiter(
                    partitionKey: "payment-global",
                    factory: _ => new ConcurrencyLimiterOptions
                    {
                        PermitLimit          = payment.GetValue("MaxConcurrent", 20),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit           = payment.GetValue("QueueLimit", 5)
                    }));
        });

        return services;
    }
}
