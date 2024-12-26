namespace HAN.Utilities;

public class SimpleMessageBroker : IMessageBroker
{
    private readonly Dictionary<Type, List<Delegate>> _subscriptions = new();

    public void Publish<TMessage>(TMessage message)
    {
        var messageType = typeof(TMessage);
        if (_subscriptions.TryGetValue(messageType, out var handlers))
        {
            foreach (var handler in handlers.Cast<Action<TMessage>>())
            {
                handler(message);
            }
        }
    }

    public void Subscribe<TMessage>(Action<TMessage> handler)
    {
        var messageType = typeof(TMessage);
        if (!_subscriptions.ContainsKey(messageType))
        {
            _subscriptions[messageType] = new List<Delegate>();
        }
        _subscriptions[messageType].Add(handler);
    }
}