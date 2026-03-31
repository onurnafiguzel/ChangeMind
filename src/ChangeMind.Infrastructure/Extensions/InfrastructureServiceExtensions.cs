namespace ChangeMind.Infrastructure.Extensions;

using ChangeMind.Application.Repositories;
using ChangeMind.Infrastructure.Data;
using ChangeMind.Infrastructure.Repositories;
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

        services.AddDbContext<ChangeMindDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsAssembly("ChangeMind.Infrastructure")));

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICoachRepository, CoachRepository>();

        return services;
    }
}
