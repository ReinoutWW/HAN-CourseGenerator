// See https://aka.ms/new-console-template for more information

using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.RabbitMQ;

Console.Title = "Blockchain Test Client (Async)";

// 1) Create or configure your RabbitMQ subscriber & publisher
IMessagePublisher publisher = new MessagePublisher("localhost");
var subscriber = new RabbitMqSubscriber("localhost");

// 2) Prepare a CancellationTokenSource for graceful shutdown
using var cts = new CancellationTokenSource();

// 3) Instantiate our handlers
var gradeSavedHandler = new GradeSavedEventHandler();
var gradeRetrievedHandler = new GradeRetrievedEventHandler();

// 4) Subscribe to relevant queues using SubscribeAsync
await subscriber.SubscribeAsync("GradeSavedQueue", gradeSavedHandler, cts.Token);
await subscriber.SubscribeAsync("GradeRetrievedQueue", gradeRetrievedHandler, cts.Token);

// (Optional) If you want to do more event handling (like "NodeHeartbeat"), 
// you can create more handlers and subscribe them here.

Console.WriteLine("=== Blockchain Test Client (Async) ===");
Console.WriteLine("Type 'save' to publish a SaveGrade message.");
Console.WriteLine("Type 'get' to publish a GetGrade message.");
Console.WriteLine("Type 'exit' to quit.\n");

// 5) Simple loop for user commands
while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine()?.Trim().ToLower();

    if (input == "exit")
    {
        Console.WriteLine("[TEST CLIENT] Exiting...");
        // Signal cancellation so that SubscribeAsync tasks can end gracefully
        cts.Cancel();
        break;
    }
    else if (input == "save")
    {
        // Prompt user for grade details
        Console.Write("StudentId: ");
        var studentId = Console.ReadLine();
        Console.Write("CourseId: ");
        var courseId = Console.ReadLine();
        Console.Write("Grade: ");
        var grade = Console.ReadLine();

        // Construct request object
        var saveGradePayload = new SaveGradeRequest
        {
            StudentId = studentId,
            CourseId = courseId,
            Grade = grade
        };

        // Wrap it in a GenericMessage
        var message = new GenericMessage
        {
            Action = "SaveGrade",
            Payload = System.Text.Json.JsonSerializer.Serialize(saveGradePayload)
        };

        // Publish to "SaveGradeQueue"
        publisher.Publish(message, "SaveGradeQueue");
        Console.WriteLine("[TEST CLIENT] Published SaveGrade message. Waiting for 'GradeSaved' event...");
    }
    else if (input == "get")
    {
        Console.Write("StudentId: ");
        var studentId = Console.ReadLine();

        var getGradePayload = new GetGradeRequest
        {
            StudentId = studentId
        };

        var message = new GenericMessage
        {
            Action = "GetGrade",
            Payload = System.Text.Json.JsonSerializer.Serialize(getGradePayload)
        };

        publisher.Publish(message, "GetGradeQueue");
        Console.WriteLine("[TEST CLIENT] Published GetGrade message. Waiting for 'GradeRetrieved' event...");
    }
    else
    {
        Console.WriteLine("Unknown command. Try 'save', 'get', or 'exit'.");
    }
}

// 6) Wait a bit to ensure everything is disposed properly
await Task.Delay(500);

Console.WriteLine("[TEST CLIENT] Exiting...");

public class SaveGradeRequest
{
    public string StudentId { get; set; }
    public string CourseId { get; set; }
    public string Grade { get; set; }
}

public class GetGradeRequest
{
    public string StudentId { get; set; }
}

public class GradeSavedEventHandler : IServiceMessageHandler<GenericMessage>
{
    public void Handle(GenericMessage message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[TEST CLIENT] Received GradeSaved event: {message.Payload}");
        Console.ResetColor();
    }
}

// Handler for "GradeRetrieved" messages
public class GradeRetrievedEventHandler : IServiceMessageHandler<GenericMessage>
{
    public void Handle(GenericMessage message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[TEST CLIENT] Received GradeRetrieved event: {message.Payload}");
        Console.ResetColor();
    }
}