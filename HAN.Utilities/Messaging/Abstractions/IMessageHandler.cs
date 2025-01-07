using HAN.Utilities.Messaging.RabbitMQ;

namespace HAN.Utilities.Messaging.Abstractions;

public interface IMessageHandler
{
    void Handle(IMessage message);
}