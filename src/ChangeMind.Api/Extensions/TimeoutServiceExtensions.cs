namespace ChangeMind.Api.Extensions;

using Microsoft.AspNetCore.Http.Timeouts;

public static class TimeoutServiceExtensions
{
    public static IServiceCollection AddTimeoutPolicies(this IServiceCollection services)
    {
        // Timeout dolunca HttpContext.RequestAborted iptal edilir; bu token
        // controller → mediator → handler → EF Core zincirinde akar.
        services.AddRequestTimeouts(options =>
        {
            options.DefaultPolicy = new RequestTimeoutPolicy
            {
                Timeout = TimeSpan.FromSeconds(8),
                TimeoutStatusCode = StatusCodes.Status504GatewayTimeout
            };

            options.AddPolicy("bulk-list", new RequestTimeoutPolicy
            {
                Timeout = TimeSpan.FromSeconds(15),
                TimeoutStatusCode = StatusCodes.Status504GatewayTimeout
            });

            options.AddPolicy("external", new RequestTimeoutPolicy
            {
                Timeout = TimeSpan.FromSeconds(25),
                TimeoutStatusCode = StatusCodes.Status504GatewayTimeout
            });
        });

        return services;
    }
}
