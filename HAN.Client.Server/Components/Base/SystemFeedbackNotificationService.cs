using Radzen;

namespace HAN.Client.Server.Components.Base;

public class SystemFeedbackNotificationService
{
    public event Action? OnSystemNotificationAdded;
    private readonly List<SystemFeedbackNotificationMessage> _systemNotifications = new();

    public IReadOnlyList<SystemFeedbackNotificationMessage> SystemNotifications => _systemNotifications.AsReadOnly();

    public void AddNotification(string text, AlertStyle type = AlertStyle.Success, int duration = 5000)
    {
        var notification = new SystemFeedbackNotificationMessage
        {
            Text = text,
            Type = type,
            Duration = duration
        };

        _systemNotifications.Add(notification);
        OnSystemNotificationAdded?.Invoke();

        Task.Delay(duration).ContinueWith(_ =>
        {
            _systemNotifications.Remove(notification);
            OnSystemNotificationAdded?.Invoke();
        });
    }
}

public class SystemFeedbackNotificationMessage
{
    public string Text { get; set; } = string.Empty;
    public AlertStyle Type { get; set; } = AlertStyle.Success; 
    public int Duration { get; set; } = 5000; 
}
