namespace HAN.Tests;

public abstract class TestBase
{
    public readonly IServiceProvider ServiceProvider;
    protected TestBase()
    {
        ServiceProvider = TestServiceProvider.BuildServiceProvider();
    }
}