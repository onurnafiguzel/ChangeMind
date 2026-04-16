namespace ChangeMind.Infrastructure.Extensions;

using ChangeMind.Application.Configuration;
using ChangeMind.Application.Repositories;
using ChangeMind.Application.Services;
using ChangeMind.Application.UnitOfWork;
using ChangeMind.Infrastructure.Data;
using ChangeMind.Infrastructure.Repositories;
using ChangeMind.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // EF Core seviyesinde son bariyer: tek bir SQL sorgusu bu süreyi geçemez.
        // Api'nin en kısa timeout'undan (default: 8s) küçük tutulur;
        // uzun süren sorgular token iptali olmadan da burada kesilir.
        var commandTimeout = configuration.GetValue("Timeout:Database:CommandTimeoutSeconds", 5);

        services.AddDbContext<ChangeMindDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly("ChangeMind.Infrastructure");
                    npgsqlOptions.CommandTimeout(commandTimeout);
                }));

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, ChangeMind.Infrastructure.UnitOfWork.UnitOfWork>();

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICoachRepository, CoachRepository>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ITrainingProgramRepository, TrainingProgramRepository>();
        services.AddScoped<IWaitingUserRepository, WaitingUserRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();

        services.AddScoped<DataSeeder>();

        // Register JWT and Security Services
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings not found in configuration.");
        services.AddSingleton(jwtSettings);
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();

        return services;
    }
}
