namespace HAN.Utilities.Messaging.Abstractions;

public class GenericMessage : IMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Action { get; set; }
    public string Payload { get; set; }
}