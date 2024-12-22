using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;

namespace HAN.Services;

public class CourseValidationService(
    ICourseService courseService,
    CourseComponentService courseComponentService
    ) : ICourseValidationService
{
    public bool ValidateCourse(int courseId)
    {
        return IsCourseComplete(courseId) && 
               HasCourseValidOrder(courseId);
    }

    public bool IsCourseComplete(int courseId)
    {
        return IsCourseScheduleComplete(courseId);
    }

    private bool IsCourseScheduleComplete(int courseId)
    {
        var course = courseService.GetCourseById(courseId);
        var schedule = courseService.GetScheduleById(course.Schedule.Id);
        var allCourseComponents = courseComponentService.GetAllCourseComponentsByCourseId(courseId);
        
        return allCourseComponents.All(courseComponent => schedule.ScheduleLines.Any(sl => sl.CourseComponentId == courseComponent.Id));
    }

    public bool HasCourseValidOrder(int courseId)
    {
        // Implement
        return true;
    }
}