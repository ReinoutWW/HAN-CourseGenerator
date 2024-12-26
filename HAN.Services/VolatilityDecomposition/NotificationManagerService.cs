namespace HAN.Services.VolatilityDecomposition;

public class NotificationManagerService : IDisposable
{
    private readonly NotificationManager _notificationManager;
    private readonly PeriodicTimer _timer;
    private readonly CancellationTokenSource _cts = new();

    public NotificationManagerService(NotificationManager notificationManager)
    {
        _notificationManager = notificationManager ?? throw new ArgumentNullException(nameof(notificationManager));
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(5)); // Check every 5 seconds
        StartProcessing();
    }

    private async void StartProcessing()
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(_cts.Token))
            {
                // Your logic for handling notifications
                Console.WriteLine("Processing notifications...");
            }
        }
        catch (OperationCanceledException)
        {
            // Graceful shutdown
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _timer.Dispose();
        _cts.Dispose();
    }
}