namespace ChangeMind.Gateway.Extensions;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ChangeMind.Application.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class SecurityServiceExtensions
{
    public static IServiceCollection AddGatewaySecurity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Get JWT settings from Gateway configuration
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings not found in Gateway configuration.");

        services.AddSingleton(jwtSettings);

        // Configure JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = "sub",
                    RoleClaimType = "role"
                };
            });

        // Configure CORS
        services.AddCors(options =>
        {
            options.AddPolicy("frontend", policy =>
                policy.WithOrigins("http://localhost:3001")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials());
        });

        // Configure Authorization Policies
        services.AddAuthorization(options =>
        {
            options.AddPolicy("auth",          policy => policy.RequireAuthenticatedUser());
            options.AddPolicy("admin-only",    policy => policy.RequireRole("Admin"));
            options.AddPolicy("coach-only",    policy => policy.RequireRole("Coach"));
            options.AddPolicy("coach-or-admin", policy => policy.RequireRole("Coach", "Admin"));
        });

        return services;
    }
}
