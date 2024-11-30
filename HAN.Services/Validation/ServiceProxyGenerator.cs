using Castle.DynamicProxy;

namespace HAN.Services.Validation;

/// <summary>
/// Utility class for generating proxy instances for interfaces.
/// </summary>
public static class ServiceProxyGenerator
{
    private static readonly ProxyGenerator ProxyGenerator = new();

    public static TInterface CreateProxy<TInterface, TImplementation>(
        TImplementation instance,
        IInterceptor interceptor
    )
        where TInterface : class
        where TImplementation : class, TInterface
    {
        return ProxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(instance, interceptor);
    }
}