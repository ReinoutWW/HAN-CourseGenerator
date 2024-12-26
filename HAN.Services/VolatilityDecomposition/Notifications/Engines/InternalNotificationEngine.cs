namespace HAN.Services.VolatilityDecomposition.Notifications.Engines;

public class InternalNotificationEngine : INotificationEngine
{
    private readonly InternalNotificationStateService _internalNotificationStateService;

    public InternalNotificationEngine(InternalNotificationStateService internalNotificationStateService)
    {
        _internalNotificationStateService = internalNotificationStateService;
    }

    public NotificationMethod Method => NotificationMethod.Internal;

    public void Notify(Notification notification)
    {
        switch (notification)
        {
            case EntityPersistedNotification entityNotification:
                HandleEntityPersistedNotification(entityNotification);
                break;

            default:
                HandleGenericNotification(notification);
                break;
        }
    }

    private void HandleEntityPersistedNotification(EntityPersistedNotification notification)
    {
        _internalNotificationStateService.AddNotification(notification);
        Console.WriteLine($"Internal Notification: {notification.PersistData.EntityName} created successfully!");
    }

    private void HandleGenericNotification(Notification notification)
    {
        _internalNotificationStateService.AddNotification(notification);
        Console.WriteLine($"Internal Notification: {notification.Title} - {notification.Message}");
    }
}
