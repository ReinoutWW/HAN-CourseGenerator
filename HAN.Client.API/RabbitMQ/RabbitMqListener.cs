using System.Diagnostics;
using HAN.Services.Messages;
using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.RabbitMQ;

namespace HAN.Client.API.RabbitMQ;

public class RabbitMqListenerService(IServiceMessageHandler<CourseMessage> courseMessageHandler, IConfiguration configuration) : RabbitMqMessageHandler<CourseMessage>(configuration)
{
    public override void Handle(IMessage message)
    {
        switch (message)
        {
            case null:
                throw new ArgumentNullException(nameof(message));
            case CourseMessage courseMessage:
                courseMessageHandler.Handle(courseMessage);
                break;
        }
        
        Console.WriteLine("> Message processed.");
    }
}