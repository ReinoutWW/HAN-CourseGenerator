using HAN.Repositories.Extensions;
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
            .AddScoped<ICourseValidationService, CourseValidationService>()
            .AddScoped<CourseComponentService>()
            .AddRepositories()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        return services;
    }
}