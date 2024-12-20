using HAN.Data;
using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using HAN.Repositories;
using HAN.Repositories.Interfaces;
using HAN.Services;
using HAN.Services.Extensions;
using HAN.Services.Interfaces;
using HAN.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Base;

public static class TestServiceProvider
{
    public static IServiceProvider BuildServiceProvider()
    {
        var inMemoryDbName = $"InMemDB-{Guid.NewGuid():N}";
        
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(inMemoryDbName));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEvlRepository, EvlRepostory>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<ICourseComponentRepository<Lesson>, CourseComponentRepository<Lesson>>();
        services.AddScoped<ICourseComponentRepository<Exam>, CourseComponentRepository<Exam>>();
        services.AddScoped<ICourseComponentRepository<CourseComponent>, CourseComponentRepository<CourseComponent>>();
        services.AddScoped<IGenericRepository<ExampleEntity>, ExampleGenericRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddCourseServices();

        return services.BuildServiceProvider();
    }
}