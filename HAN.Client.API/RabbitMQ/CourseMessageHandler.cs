using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Services.Messages;
using HAN.Utilities.Messaging.Abstractions;

namespace HAN.Client.API.RabbitMQ;

public class CourseMessageHandler(ICourseService courseService) : IServiceMessageHandler<CourseMessage>
{
    public void Handle(CourseMessage message)
    {
        switch (message.CourseAction)
        {
            case CourseAction.CreateCourse:
                CreateCourse(message);
                break;
            case CourseAction.GetCourses:
            default:
                return;
        }
    }

    private void CreateCourse(CourseMessage message)
    {
        var course = System.Text.Json.JsonSerializer.Deserialize<CourseDto>(message.Payload);
        if (course == null)
            throw new Exception($"Message payload for {nameof(message)} is not correct.");

        courseService.CreateCourse(course);
    }
}