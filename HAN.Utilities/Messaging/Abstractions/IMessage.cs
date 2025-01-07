namespace HAN.Utilities.Messaging.Abstractions;

public interface IMessage
{
    public string Id { get; set; }
    public string Action { get; set; }
    public string Payload { get; set; }
}