using System.Text.Json.Serialization;
using ChangeMind.Api.Extensions;
using ChangeMind.Api.Middleware;
using ChangeMind.Application.Extensions;
using ChangeMind.Infrastructure.Data;
using ChangeMind.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddTimeoutPolicies();
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
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

app.UseExceptionHandler();
app.UseRequestTimeouts();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
app.MapHealthCheckEndpoints();

app.Run();
