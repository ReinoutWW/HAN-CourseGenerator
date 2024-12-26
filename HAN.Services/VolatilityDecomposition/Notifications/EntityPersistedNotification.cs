namespace HAN.Services.VolatilityDecomposition.Notifications;

public class EntityPersistedNotification : Notification
{
    public EntityPersistedData PersistData { get; set; }

    public override void Execute()
    {
        Console.WriteLine($"{PersistData.EntityName} created successfully with id: {PersistData.Entity.Id}");
    }
}