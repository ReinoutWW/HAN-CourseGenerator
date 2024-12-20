using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using HAN.Services.Validation;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddScoped<LessonService>();
        services.AddScoped<ExamService>();
        
        return services;
    }
}