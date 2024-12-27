namespace HAN.Services.VolatilityDecomposition;

public abstract class Notification() : IExecutable
{
    public string Title { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual void Execute()
    {
        // Default execution logic, if any
    }
}