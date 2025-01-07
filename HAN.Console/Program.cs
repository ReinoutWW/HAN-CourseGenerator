// See https://aka.ms/new-console-template for more information

using HAN.Console;
using HAN.Services.Messages;
using HAN.Utilities.Messaging.RabbitMQ;

MessagePublisher messagePublisher = new("localhost");

var running = true;
while (running)
{
    var course = new Course
    {
        Name = "HAN I-OOSE",
        Description = "The magical course "
    };

    var message = new CourseMessage
    {
        CourseAction = CourseAction.CreateCourse,
        Action = "CreateCourse",
        Payload = System.Text.Json.JsonSerializer.Serialize(course)
    };

    messagePublisher.Publish(message, "CourseAPI");

    Console.WriteLine("Message sent!");
    var res = Console.ReadLine();

    running = res != "exit";
}