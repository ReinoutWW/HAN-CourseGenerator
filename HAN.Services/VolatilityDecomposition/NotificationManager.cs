using HAN.Utilities;

namespace HAN.Services.VolatilityDecomposition;

public class NotificationManager
{
    private readonly Func<IUserPreferences> _userPreferencesFactory;
    private readonly NotificationMethodRegistry _methodRegistry;
    private readonly IMessageBroker _messageBroker;

    public NotificationManager(
        Func<IUserPreferences> userPreferencesFactory,
        NotificationMethodRegistry methodRegistry,
        IMessageBroker messageBroker)
    {
        _userPreferencesFactory = userPreferencesFactory;
        _methodRegistry = methodRegistry;
        _messageBroker = messageBroker;

        _messageBroker.Subscribe<NotificationEvent>(OnNotificationReceived);
    }

    private void OnNotificationReceived(NotificationEvent notificationEvent)
    {
        var userPreferences = _userPreferencesFactory().Preferences;
        if (userPreferences.TryGetValue(notificationEvent.Type, out var methods))
        {
            foreach (var method in methods)
            {
                var engine = _methodRegistry.GetEngine(method);
                engine.Notify(notificationEvent.Notification);
            }
        }
    }
}
