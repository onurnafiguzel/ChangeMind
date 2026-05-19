using System.Text.Json.Serialization;
using ChangeMind.Api.Extensions;
using ChangeMind.Api.Middleware;
using ChangeMind.Application.Extensions;
using ChangeMind.Infrastructure.Data;
using ChangeMind.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, services, config) =>
    config.ReadFrom.Configuration(ctx.Configuration)
          .ReadFrom.Services(services)
          .Enrich.FromLogContext());

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddTimeoutPolicies();
builder.Services.AddBulkhead(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddHealthCheckServices(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
builder.Services.AddApiSecurity(builder.Configuration);
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChangeMindDbContext>();
    await db.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseExceptionHandler();
app.UseRequestTimeouts();
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpMetrics();
app.MapMetrics("/metrics");

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

// Static file serving for uploaded user photos (path matches LocalDiskPhotoStorageService.PublicUrlPrefix).
var photosRoot = builder.Configuration["Storage:PhotosRoot"]
    ?? Path.Combine(AppContext.BaseDirectory, "wwwroot", "uploads", "photos");
Directory.CreateDirectory(photosRoot);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(photosRoot),
    RequestPath  = "/uploads/photos"
});

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthCheckEndpoints();

app.Run();
