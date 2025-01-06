namespace HAN.Console;

public class CourseService
{
    private readonly RabbitMqPublisherWrapper _publisherWrapper = new("localhost");

    public void CreateCourse(Course course, string accessToken)
    {
        var message = new Message()
        {
            Endpoint = "Course",
            Method = "POST",
            Payload = System.Text.Json.JsonSerializer.Serialize(course),
            AuthToken = accessToken
        };

        var jsonMessage = System.Text.Json.JsonSerializer.Serialize(message);
        
        _ = _publisherWrapper.PublishAsync(jsonMessage);
    }
}