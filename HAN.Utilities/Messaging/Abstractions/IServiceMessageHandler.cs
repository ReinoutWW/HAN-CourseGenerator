namespace HAN.Utilities.Messaging.Abstractions;

public interface IServiceMessageHandler<in TMessage> where TMessage : IMessage
{
    public void Handle(TMessage message);
}