using System.Text;
using System.Text.Json;
using HAN.Utilities.Messaging.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HAN.Utilities.Messaging.RabbitMQ;

public abstract class RabbitMqMessageHandler<TMessage> : IMessageBackgroundListenerService where TMessage : IMessage
{
    public async void Listen(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        await using var connection = await factory.CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: "CourseAPI",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);
            var messageObject = JsonSerializer.Deserialize<TMessage>(messageJson);

            Handle(messageObject);
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(
            queue: "CourseAPI",
            autoAck: true,
            consumer: consumer, cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public abstract void Handle(IMessage handler);
}