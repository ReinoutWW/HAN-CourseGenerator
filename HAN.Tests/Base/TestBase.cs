using HAN.Data;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Base;

public abstract class TestBase : IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly AppDbContext Context;

    protected TestBase()
    {
        _scope = TestServiceProvider.BuildServiceProvider().CreateScope();
        
        ServiceProvider = _scope.ServiceProvider;
        Context = ServiceProvider.GetRequiredService<AppDbContext>();
        
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _scope.Dispose(); // Dispose scope and associated resources
        }
    }
}