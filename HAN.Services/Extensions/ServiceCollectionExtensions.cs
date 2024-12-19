using HAN.Services.Validation;
using Microsoft.Extensions.DependencyInjection;
using File = HAN.Data.Entities.File;

namespace HAN.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCourseServices(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IEvlService, EvlService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFileService, FileService>();
        return services;
    }
}