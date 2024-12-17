using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories;
using HAN.Repositories.Interfaces;
using HAN.Services;
using HAN.Services.Extensions;
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
        services.AddScoped<ICourseComponentRepository, CourseComponentRepository>();
        services.AddScoped<IGenericRepository<ExampleEntity>, ExampleGenericRepository>();
        services.AddScoped<ICourseValidationService, CourseValidationService>();
        services.AddScoped<ICoursePlanningValidationService, CoursePlanningValidationService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddCourseServices();

        return services.BuildServiceProvider();
    }
}