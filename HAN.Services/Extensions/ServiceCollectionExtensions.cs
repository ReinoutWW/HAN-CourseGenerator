using HAN.Services.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCourseServices(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ICourseComponentService, CourseComponentService>();
        services.AddScoped<IEvlService, EvlService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}