namespace HAN.Client.API.RabbitMQ;

public class Message
{
    public string Endpoint { get; set; }
    public string Method { get; set; } = "GET";
    public string Payload { get; set; }
    public string? AuthToken { get; set; }
}