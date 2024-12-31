using HAN.Data;
using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Repositories.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services = services.AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IEvlRepository, EvlRepostory>()
            .AddScoped<ICourseRepository, CourseRepository>()
            .AddScoped<IFileRepository, FileRepository>()
            .AddScoped<IScheduleRepository, ScheduleRepository>()
            .AddScoped<ICourseComponentRepository<Lesson>, CourseComponentRepository<Lesson>>()
            .AddScoped<ICourseComponentRepository<Exam>, CourseComponentRepository<Exam>>()
            .AddScoped<ICourseComponentRepository<CourseComponent>, CourseComponentRepository<CourseComponent>>();
        
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseInMemoryDatabase("InMem"));
        
        return services;
    }
}