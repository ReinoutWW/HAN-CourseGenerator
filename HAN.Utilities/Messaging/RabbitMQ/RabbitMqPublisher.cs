using System.Text;
using HAN.Utilities.Messaging.Abstractions;
using RabbitMQ.Client;

namespace HAN.Utilities.Messaging.RabbitMQ;

public class RabbitMqPublisher(string hostname) : IMessagePublisher
{
    private readonly ConnectionFactory _factory = new() { HostName = hostname };

    public async void Publish<TMessage>(TMessage message, string queueName) where TMessage : IMessage
    {
        await using var connection = await _factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var jsonMessage = System.Text.Json.JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        await channel.BasicPublishAsync(
            exchange: string.Empty, 
            routingKey: queueName,
            body: body
        );
    }
}