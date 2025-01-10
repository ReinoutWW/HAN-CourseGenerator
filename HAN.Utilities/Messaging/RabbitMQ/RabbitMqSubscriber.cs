using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using HAN.Utilities.Messaging.Abstractions;
using RabbitMQ.Client.Events;

namespace HAN.Utilities.Messaging.RabbitMQ;

public class RabbitMqSubscriber
{
    private readonly string _hostName;
    private IConnection? _connection;

    public RabbitMqSubscriber(string hostName)
    {
        _hostName = hostName;
    }

    /// <summary>
    /// Subscribes to the specified queue and handles incoming messages
    /// using the provided <paramref name="handler"/>.
    /// </summary>
    /// <typeparam name="TMessage">Type of the message that implements IMessage.</typeparam>
    /// <param name="queueName">RabbitMQ queue name.</param>
    /// <param name="handler">Implementation of IServiceMessageHandler&lt;TMessage&gt;.</param>
    /// <param name="stoppingToken">Cancellation token to handle graceful shutdown.</param>
    public async Task SubscribeAsync<TMessage>(
        string queueName, 
        IServiceMessageHandler<TMessage> handler, 
        CancellationToken stoppingToken = default
    )
        where TMessage : IMessage
    {
        // Create a connection factory with the desired host
        var factory = new ConnectionFactory
        {
            HostName = _hostName
            // You can specify other properties here (Username, Password, etc.)
        };

        // Create a connection and channel asynchronously
        _connection = await factory.CreateConnectionAsync(stoppingToken)
                                   .ConfigureAwait(false);

        // NOTE: The RabbitMQ.Client library's CreateChannelAsync extension is in "RabbitMQ.Client.GracefulShutdown" 
        // or your own extension. If not available, you can do this synchronously:
        // var channel = _connection.CreateModel();
        // but we'll assume you have an async method:
        var channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken)
                                       .ConfigureAwait(false);

        // Declare the queue if it doesn't exist
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        ).ConfigureAwait(false);

        // Create an async consumer
        var consumer = new AsyncEventingBasicConsumer(channel);

        // Attach the event handler for receiving messages
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                // Deserialize the incoming message
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                // We expect a JSON string that corresponds to TMessage
                var messageObject = JsonSerializer.Deserialize<TMessage>(messageJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (messageObject != null)
                {
                    // Hand off to the provided handler
                    handler.Handle(messageObject);
                }
                else
                {
                    Console.WriteLine($"[RabbitMqSubscriber] Failed to deserialize message from queue '{queueName}'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RabbitMqSubscriber] Exception while handling message: {ex.Message}");
            }

            // Since we're using autoAck=true in BasicConsumeAsync, 
            // the broker won't wait for an ACK.
            await Task.CompletedTask;
        };

        // Start consuming
        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: true,  // or false if you want to manually ACK
            consumer: consumer,
            cancellationToken: stoppingToken
        ).ConfigureAwait(false);

        Console.WriteLine($"[RabbitMqSubscriber] Subscribed to queue '{queueName}'");
    }
}
