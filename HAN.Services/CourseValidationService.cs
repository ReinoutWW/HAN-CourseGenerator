using HAN.Services.DTOs;
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
        var evlIds = course.Evls.Select(evl => evl.Id).ToList();
        
        var allCourseComponents = courseComponentService.GetAllCourseComponentByEvlIds(evlIds);
        
        return allCourseComponents.All(
                courseComponent => schedule.ScheduleLines.Any(sl => sl.CourseComponentId == courseComponent.Id)
            );
    }

    public bool HasCourseValidOrder(int courseId)
    {
        var course = courseService.GetCourseById(courseId);
        var schedule = courseService.GetScheduleById(course.Schedule.Id);

        return course.Evls.All(evl => EvlHasValidOrder(evl, schedule));
    }
    
    private bool EvlHasValidOrder(EvlDto evl, ScheduleDto schedule)
    {
        var courseComponents = courseComponentService.GetAllCourseComponentsByEvlId(evl.Id);
        
        // Foreach component, check if it is in the schedule
        // If eam, check if there's ANY Lessons AFTER exam
        // If lesson, check if there's ANY Exams BEFORE lesson
        
        // Code:
        foreach (var courseComponent in courseComponents)
        {
            var scheduleLine = schedule.ScheduleLines.FirstOrDefault(sl => sl.CourseComponentId == courseComponent.Id);
            if (scheduleLine == null)
                continue;
            
            if (courseComponent is ExamDto)
            {
                var lessonsAfterExam = schedule.ScheduleLines
                    .Where(sl => sl.CourseComponent is LessonDto)
                    .Where(lesson => lesson.WeekSequenceNumber > scheduleLine.WeekSequenceNumber)
                    .Select(lesson => lesson.CourseComponentId);
                
                if (lessonsAfterExam.Any())
                    return false;
            }
        }
        
        return true;
    }
}