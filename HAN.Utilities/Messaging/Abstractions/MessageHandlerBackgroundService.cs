using Microsoft.Extensions.Hosting;

namespace HAN.Utilities.Messaging.Abstractions;

public class MessageHandlerBackgroundService(IMessageBackgroundListenerService messageHandler) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        messageHandler.Listen(stoppingToken);
        return Task.CompletedTask;
    }
}