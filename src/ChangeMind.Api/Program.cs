using System.Text.Json.Serialization;
using ChangeMind.Api.Middleware;
using ChangeMind.Application.Extensions;
using ChangeMind.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.Timeouts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Timeout policies.
// Timeout dolunca HttpContext.RequestAborted iptal edilir; bu token
// controller CancellationToken parametresine, oradan mediator.Send'e,
// handler'a ve EF Core sorgularına akar — tüm in-flight işlemler kesilir.
builder.Services.AddRequestTimeouts(options =>
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

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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

app.UseExceptionHandler();

// UseRequestTimeouts, controller action çalışmadan önce devreye girer.
// Süre dolunca HttpContext.RequestAborted iptal edilir ve TimeoutStatusCode (504) ile yanıt döner.
app.UseRequestTimeouts();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.Run();
