namespace HAN.Utilities;

public interface IMessageBroker
{
    void Publish<TMessage>(TMessage message);
    void Subscribe<TMessage>(Action<TMessage> handler);
}