using ChangeMind.Gateway.Extensions;
using Microsoft.AspNetCore.Http.Timeouts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGatewaySecurity(builder.Configuration);
builder.Services.AddGatewayRateLimiting(builder.Configuration);

// Timeout policies — her route appsettings.json'da "TimeoutPolicy" ile birini seçer.
// Gateway süresi her zaman Api'nin karşılık gelen policy süresinden büyük olmalı.
builder.Services.AddRequestTimeouts(options =>
{
    //Api default: 8s < Gateway default: 20s
    options.DefaultPolicy = new RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(20),
        TimeoutStatusCode = StatusCodes.Status504GatewayTimeout
    };

    // Api default: 8s < Gateway strict: 10s
    options.AddPolicy("strict", new RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(10),
        TimeoutStatusCode = StatusCodes.Status504GatewayTimeout
    });

    // Api bulk-list: 15s < Gateway bulk-list: 20s
    // Gateway default (20s) bu route'ları da kapsar; ayrı policy gerekmez.

    // Api external: 25s < Gateway extended: 30s
    options.AddPolicy("extended", new RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(30),
        TimeoutStatusCode = StatusCodes.Status504GatewayTimeout
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("auth", policy =>
        policy.RequireAuthenticatedUser());

    options.AddPolicy("admin-only", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("coach-or-admin", policy =>
        policy.RequireRole("Coach", "Admin"));

    options.AddPolicy("coach-only", policy =>
        policy.RequireRole("Coach"));
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseRateLimiter();

// UseRequestTimeouts, UseAuthentication'dan önce: kimlik doğrulamaya bile zaman vermiyor — saldırgan tokenlar uzun süre bloke edemez.
app.UseRequestTimeouts();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();
