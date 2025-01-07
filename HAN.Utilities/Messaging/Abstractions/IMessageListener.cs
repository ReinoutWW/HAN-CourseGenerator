namespace HAN.Utilities.Messaging.Abstractions;

public interface IMessageListener
{
    void Listen(CancellationToken stoppingToken);
}