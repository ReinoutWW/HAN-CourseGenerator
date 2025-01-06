using HAN.Services;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;

namespace HAN.Client.API;

public static class CourseApi
{
    public static void MapCourseEndpoints(this WebApplication app)
    {
        app.MapGet("/course", (ICourseService courseService) =>
        {
            var courses = courseService.GetAllCourses();
            return Results.Ok(courses);
        }).RequireAuthorization();
        
        app.MapPost("/course", (ICourseService courseService, CourseDto course) =>
        {
            courseService.CreateCourse(course);
            return Results.Created();
        }).RequireAuthorization();
    }
}