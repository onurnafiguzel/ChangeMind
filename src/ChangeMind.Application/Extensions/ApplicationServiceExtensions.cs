namespace ChangeMind.Application.Extensions;

using System.Reflection;
using ChangeMind.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationServiceExtensions).Assembly;

        // MediatR handlers
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(assembly));

        // FluentValidation — tüm validator'ları assembly taramasıyla kaydet
        services.AddValidatorsFromAssembly(assembly);

        // MediatR pipeline: her request handler çalışmadan önce validation çalışır
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
