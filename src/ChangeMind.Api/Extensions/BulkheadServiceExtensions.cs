namespace ChangeMind.Api.Extensions;

using System.Threading.RateLimiting;
using ChangeMind.Application.Configuration;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class BulkheadServiceExtensions
{
    public static IServiceCollection AddBulkhead(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var opts = configuration.GetSection("Bulkhead").Get<BulkheadOptions>() ?? new BulkheadOptions();

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status503ServiceUnavailable;

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.Headers["Retry-After"] = "5";
                await context.HttpContext.Response.WriteAsync(
                    "Payment service is at capacity. Please retry in 5 seconds.", token);
            };

            // Payment bulkhead — eş zamanlı işlem sayısını sınırlar
            // Rate limiting'den farklı: zaman penceresi değil, anlık slot sayısı kontrol edilir
            options.AddConcurrencyLimiter("payment-bulkhead", limiterOptions =>
            {
                limiterOptions.PermitLimit = opts.PaymentMaxConcurrent;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = opts.PaymentQueueLimit;
            });
        });

        return services;
    }
}
