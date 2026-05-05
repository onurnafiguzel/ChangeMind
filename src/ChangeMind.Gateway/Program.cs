using ChangeMind.Gateway.Extensions;
using ChangeMind.Gateway.Middleware;
using Microsoft.AspNetCore.Http.Timeouts;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, services, config) =>
    config.ReadFrom.Configuration(ctx.Configuration)
          .ReadFrom.Services(services)
          .Enrich.FromLogContext());

builder.Services.AddGatewaySecurity(builder.Configuration);
builder.Services.AddGatewayRateLimiting(builder.Configuration);

builder.Services.AddRequestTimeouts(options =>
{
    options.DefaultPolicy = new RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(20),
        TimeoutStatusCode = StatusCodes.Status504GatewayTimeout
    };

    options.AddPolicy("default", new RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(20),
        TimeoutStatusCode = StatusCodes.Status504GatewayTimeout
    });

    options.AddPolicy("strict", new RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(10),
        TimeoutStatusCode = StatusCodes.Status504GatewayTimeout
    });

    options.AddPolicy("extended", new RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(30),
        TimeoutStatusCode = StatusCodes.Status504GatewayTimeout
    });
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("auth",          p => p.RequireAuthenticatedUser())
    .AddPolicy("admin-only",    p => p.RequireRole("Admin"))
    .AddPolicy("coach-or-admin",p => p.RequireRole("Coach", "Admin"))
    .AddPolicy("coach-only",    p => p.RequireRole("Coach"));

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.MapHealthChecks("/health");

app.UseHttpMetrics();
app.MapMetrics("/metrics");

app.UseRateLimiter();
app.UseRequestTimeouts();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("frontend");

app.MapReverseProxy();

app.Run();
