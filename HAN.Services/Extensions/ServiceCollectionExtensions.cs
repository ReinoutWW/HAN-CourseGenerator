using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using HAN.Services.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCourseServices(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>()
            .AddScoped<ICourseService, CourseService>()
            .AddScoped<IEvlService, EvlService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IFileService, FileService>()
            .AddScoped<LessonService>()
            .AddScoped<ExamService>()
            .AddScoped<IScheduleService, ScheduleService>()
            .AddScoped<ICourseValidationService, CourseValidationService>()
            .AddScoped<CourseComponentService>();
        
        return services;
    }
}