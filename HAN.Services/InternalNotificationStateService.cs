using HAN.Services.VolatilityDecomposition;

namespace HAN.Services;

public class InternalNotificationStateService
{
    private readonly List<Notification> _notifications = [];

    public IReadOnlyList<Notification> Notifications => _notifications.AsReadOnly();

    public event Action? OnNotificationsChanged;

    public void AddNotification(Notification notification)
    {
        _notifications.Add(notification);
        NotifyStateChanged();
    }

    public void ClearNotifications()
    {
        _notifications.Clear();
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnNotificationsChanged?.Invoke();
}
