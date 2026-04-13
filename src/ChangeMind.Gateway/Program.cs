using ChangeMind.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGatewaySecurity(builder.Configuration);

// Add authorization policies for route-based auth
builder.Services.AddAuthorization(options =>
{
    // Auth - token required, any authenticated user
    options.AddPolicy("auth", policy =>
        policy.RequireAuthenticatedUser());

    // Admin only
    options.AddPolicy("admin-only", policy =>
        policy.RequireRole("Admin"));

    // Coach or Admin
    options.AddPolicy("coach-or-admin", policy =>
        policy.RequireRole("Coach", "Admin"));
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();
