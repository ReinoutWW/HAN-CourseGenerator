namespace HAN.Services.VolatilityDecomposition.Notifications.Engines;

public class EmailNotificationEngine : INotificationEngine
{
    public NotificationMethod Method => NotificationMethod.Email;

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
        Console.WriteLine($"Email sent: {notification.PersistData.EntityName} created successfully!");
    }

    private void HandleGenericNotification(Notification notification)
    {
        Console.WriteLine($"Email sent: {notification.Title} - {notification.Message}");
    }
}
