using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using HAN.Utilities.Messaging.Abstractions;
using RabbitMQ.Client.Events;

namespace HAN.Utilities.Messaging.RabbitMQ;

public class RabbitMqSubscriber
{
    private readonly string _hostName;
    private readonly string _nodeId;
    private IConnection? _connection;

    public RabbitMqSubscriber(string hostName, string nodeId)
    {
        _hostName = hostName;
        _nodeId = nodeId;
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
        var factory = new ConnectionFactory { HostName = _hostName };

        _connection = await factory.CreateConnectionAsync(stoppingToken)
                                   .ConfigureAwait(false);

        var channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken)
                                       .ConfigureAwait(false);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken
        ).ConfigureAwait(false);

        var consumer = new AsyncEventingBasicConsumer(channel);

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
                
                if (messageObject == null)
                    return;

                if (IsOwnMessage(messageObject))
                    return;
                
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);

                handler.Handle(messageObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RabbitMqSubscriber] Exception while handling message: {ex.Message}");
            }

            await Task.CompletedTask;
        };

        // Start consuming
        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,  // or false if you want to manually ACK
            consumer: consumer,
            cancellationToken: stoppingToken
        ).ConfigureAwait(false);

        Console.WriteLine($"[RabbitMqSubscriber] Subscribed to queue '{queueName}'");
    }

    private bool IsOwnMessage(IMessage message) => message.NodeId == _nodeId;
}
