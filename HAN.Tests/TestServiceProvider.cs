using HAN.Data;
using HAN.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests;

public class TestServiceProvider
{
    private const string InMemoryDbName = "TestDb";
    
    public static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(InMemoryDbName));

        services.AddScoped<IUserRepository, UserRepository>();

        return services.BuildServiceProvider();
    }
}