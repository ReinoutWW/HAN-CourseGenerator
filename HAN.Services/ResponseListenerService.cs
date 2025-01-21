using HAN.Services.Interfaces;
using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.RabbitMQ;

namespace HAN.Services;

public class ResponseListenerService : IResponseListener
{
    // Implement the events
    public event EventHandler<GradeSavedEventArgs> GradeSavedReceived;
    public event EventHandler<GradeRetrievedEventArgs> GradeRetrievedReceived;
    public event EventHandler<NodeMonitoringQueueEventArgs>? NodeMonitoringQueueReceived;
    public event EventHandler<GetBlockResponseEventArgs> GetBlockResponseReceived;

    private readonly RabbitMqSubscriber _subscriber;

    public ResponseListenerService(RabbitMqSubscriber subscriber)
    {
        _subscriber = subscriber;

        _subscriber.SubscribeAsync<GenericMessage>(
            "GradeSavedQueue",
            new GenericMessageHandler(OnGradeSaved),
            default
        );

        _subscriber.SubscribeAsync<GenericMessage>(
            "GradeRetrievedQueue",
            new GenericMessageHandler(OnGradeRetrieved),
            default
        );

        _subscriber.SubscribeAsync<GenericMessage>(
            "NodeMonitoringQueue",
            new GenericMessageHandler(OnNodeMonitoringQueueReceived),
            default
        );
        
        _subscriber.SubscribeAsync<GenericMessage>(
            "BlockRetrievedQueue",
            new GenericMessageHandler(OnBlockRetrieved),
            default
        );
    }
    
    private void OnBlockRetrieved(GenericMessage message)
    {
        GetBlockResponseReceived?.Invoke(this, new GetBlockResponseEventArgs
        {
            MessageId = message.Id,
            PayloadJson = message.Payload
        });
    }

    private void OnGradeSaved(GenericMessage message)
    {
        GradeSavedReceived?.Invoke(this, new GradeSavedEventArgs
        {
            MessageId = message.Id,
            PayloadJson = message.Payload
        });
    }

    private void OnGradeRetrieved(GenericMessage message)
    {
        GradeRetrievedReceived?.Invoke(this, new GradeRetrievedEventArgs
        {
            MessageId = message.Id,
            PayloadJson = message.Payload
        });
    }

    private void OnNodeMonitoringQueueReceived(GenericMessage message)
    {
        NodeMonitoringQueueReceived?.Invoke(this, new NodeMonitoringQueueEventArgs
        {
            MessageId = message.Id,
            PayloadJson = message.Payload
        });
    }

    // Helper class implementing IServiceMessageHandler<GenericMessage> 
    // so we can pass a lambda
    private class GenericMessageHandler : IServiceMessageHandler<GenericMessage>
    {
        private readonly Action<GenericMessage> _action;
        public GenericMessageHandler(Action<GenericMessage> action) => _action = action;
        public void Handle(GenericMessage message) => _action(message);
    }
}