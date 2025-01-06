using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HAN.Client.API.RabbitMQ;

public class RabbitMqListenerService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var messageObject = JsonSerializer.Deserialize<Message>(messageJson);

                if (messageObject is null)
                    return; // Or handle invalid message

                // Create a dispatcher instance
                var httpClient = new HttpClient();
                var dispatcher = new MessageDispatcher(httpClient);

                // Dispatch the message to the endpoint
                var responseContent = await dispatcher.DispatchAsync(messageObject);

                Console.WriteLine($"Dispatched to {messageObject.Endpoint} - Response: {responseContent}");
            }
            catch (Exception ex)
            {
                // Log or handle any error
                Console.Error.WriteLine(ex.ToString());
            }
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
}