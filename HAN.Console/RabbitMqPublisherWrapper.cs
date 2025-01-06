using System.Text;
using RabbitMQ.Client;

namespace HAN.Console;

public class RabbitMqPublisherWrapper(string hostname)
{
    private readonly ConnectionFactory _factory = new() { HostName = hostname };

    public async Task PublishAsync(string message)
    {
        await using var connection = await _factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "CourseAPI",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(
            exchange: string.Empty, 
            routingKey: "CourseAPI",
            body: body
        );
    }
}