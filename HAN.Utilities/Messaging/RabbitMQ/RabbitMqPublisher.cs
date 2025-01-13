using System.Text;
using HAN.Utilities.Messaging.Abstractions;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace HAN.Utilities.Messaging.RabbitMQ;

public class RabbitMqPublisher(IConfiguration configuration, string nodeId) : IMessagePublisher
{
    public async void Publish<TMessage>(TMessage message, string queueName) where TMessage : IMessage
    {
        ConnectionFactory _factory = configuration.CreateConnectionFactory();
        
        message.NodeId = nodeId;
        
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