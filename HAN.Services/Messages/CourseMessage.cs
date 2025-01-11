using DocumentFormat.OpenXml.Wordprocessing;
using HAN.Utilities.Messaging.Abstractions;

namespace HAN.Services.Messages;

public class CourseMessage : IMessage
{
    public string NodeId { get; set; } = string.Empty;
    public string Id { get; set; } = Guid.NewGuid().ToString("n");
    public CourseAction CourseAction { get; set; }
    public string Action { get; set; }
    public string Payload { get; set; }
}