namespace HAN.Utilities.Messaging.Abstractions;

public interface IMessagePublisher
{
    void Publish<TMessage>(TMessage message, string queueName) where TMessage : IMessage;
}