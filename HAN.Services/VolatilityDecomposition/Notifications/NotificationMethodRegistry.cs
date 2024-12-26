using HAN.Services.VolatilityDecomposition.Notifications;
using HAN.Services.VolatilityDecomposition.Notifications.Engines;

namespace HAN.Services.VolatilityDecomposition;

public class NotificationMethodRegistry
{
    private readonly Dictionary<NotificationMethod, INotificationEngine> _engineMappings;

    public NotificationMethodRegistry(IEnumerable<INotificationEngine> engines)
    {
        _engineMappings = engines.ToDictionary(e => e.Method);
    }

    public INotificationEngine GetEngine(NotificationMethod method)
    {
        return _engineMappings.TryGetValue(method, out var engine)
            ? engine
            : throw new NotImplementedException($"No engine found for method {method}");
    }
}