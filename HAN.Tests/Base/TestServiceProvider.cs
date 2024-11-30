using HAN.Data;
using HAN.Repositories;
using HAN.Services;
using HAN.Services.Extensions;
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
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddCourseServices();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services.BuildServiceProvider();
    }
}