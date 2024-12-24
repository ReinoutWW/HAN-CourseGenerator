using Radzen;

namespace HAN.Client.Components.Base;

public class NotificationService
{
    public event Action? OnNotificationAdded;
    private readonly List<NotificationMessage> _notifications = new();

    public IReadOnlyList<NotificationMessage> Notifications => _notifications.AsReadOnly();

    public void AddNotification(string text, AlertStyle type = AlertStyle.Success, int duration = 5000)
    {
        var notification = new NotificationMessage
        {
            Text = text,
            Type = type,
            Duration = duration
        };

        _notifications.Add(notification);
        OnNotificationAdded?.Invoke();

        Task.Delay(duration).ContinueWith(_ =>
        {
            _notifications.Remove(notification);
            OnNotificationAdded?.Invoke();
        });
    }
}

public class NotificationMessage
{
    public string Text { get; set; } = string.Empty;
    public AlertStyle Type { get; set; } = AlertStyle.Success; 
    public int Duration { get; set; } = 5000; 
}
