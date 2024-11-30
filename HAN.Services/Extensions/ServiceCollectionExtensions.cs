using HAN.Services.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCourseServices(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>();
        services.AddProxiedValidation<ICourseService, CourseService>();
        return services;
    }
    
    private static IServiceCollection AddProxiedValidation<TInterface, TImplementation>(
        this IServiceCollection services
    )
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddScoped<TImplementation>(); // Register the implementation
        services.AddScoped<TInterface>(provider =>
        {
            var implementation = provider.GetRequiredService<TImplementation>();
            var validationService = provider.GetRequiredService<IValidationService>();
            var interceptor = new ValidationInterceptor(validationService);
            return ServiceProxyGenerator.CreateProxy<TInterface, TImplementation>(implementation, interceptor);
        });

        return services;
    }
}