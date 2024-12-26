namespace HAN.Services.VolatilityDecomposition.Notifications.Engines;

public interface INotificationEngine
{
    NotificationMethod Method { get; }
    void Notify(Notification notification);
}