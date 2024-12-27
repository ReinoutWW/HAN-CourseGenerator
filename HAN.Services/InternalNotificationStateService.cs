using HAN.Services.VolatilityDecomposition;

namespace HAN.Services;

public class InternalNotificationStateService
{
    private readonly List<Notification> _notifications = [];
    private readonly List<Notification> _readNotifications = [];

    public IReadOnlyList<Notification> Notifications => _notifications.AsReadOnly();

    public event Action? OnNotificationsChanged;

    public void AddNotification(Notification notification)
    {
        _notifications.Add(notification);
        NotifyStateChanged();
    }

    public void ReadAll()
    {
        _readNotifications.AddRange(_notifications);
        NotifyStateChanged();
    }
    
    public void MarkAsRead(Notification notification)
    {
        if (_readNotifications.Contains(notification))
            return;
        
        _readNotifications.Add(notification);
        NotifyStateChanged();
    }
    
    public bool IsNotificationRead(Notification notification)
    {
        return _readNotifications.Contains(notification);
    }

    public void ClearNotifications()
    {
        _notifications.Clear();
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnNotificationsChanged?.Invoke();
}
