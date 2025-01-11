using HAN.Services.Interfaces;
using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.RabbitMQ;

namespace HAN.Services;

public class ResponseListenerService : IResponseListener
{
    // Implement the events
    public event EventHandler<GradeSavedEventArgs> GradeSavedReceived;
    public event EventHandler<GradeRetrievedEventArgs> GradeRetrievedReceived;
    public event EventHandler<NodeListResponseEventArgs> NodeListResponseReceived;

    private readonly RabbitMqSubscriber _subscriber;

    public ResponseListenerService(RabbitMqSubscriber subscriber)
    {
        _subscriber = subscriber;

        // Subscribe to "GradeSavedQueue"
        // We'll parse messages with "Action = GradeSaved"
        _subscriber.SubscribeAsync<GenericMessage>(
            "GradeSavedQueue",
            new GenericMessageHandler(OnGradeSaved),
            default
        );

        // Subscribe to "GradeRetrievedQueue"
        _subscriber.SubscribeAsync<GenericMessage>(
            "GradeRetrievedQueue",
            new GenericMessageHandler(OnGradeRetrieved),
            default
        );

        // Similarly for a "NodeListResponse" queue,
        // or you might reuse the "NodeMonitoringQueue"
        _subscriber.SubscribeAsync<GenericMessage>(
            "NodeListResponseQueue",
            new GenericMessageHandler(OnNodeListResponse),
            default
        );
    }

    private void OnGradeSaved(GenericMessage message)
    {
        // Raise the event
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

    private void OnNodeListResponse(GenericMessage message)
    {
        NodeListResponseReceived?.Invoke(this, new NodeListResponseEventArgs
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