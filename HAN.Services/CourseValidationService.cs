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
        var course = courseService.GetCourseById(courseId);
        if (course.Schedule == null)
        {
            throw new InvalidOperationException("Course does not have a schedule.");
        }

        return IsCourseComplete(course) && HasCourseValidOrder(course);
    }

    public bool ValidateCourse(CourseDto course)
    {
        if (course.Schedule == null)
            return false;

        return IsCourseComplete(course) && HasCourseValidOrder(course);
    }

    public bool IsCourseComplete(CourseDto courseDto)
    {
        if (courseDto.Schedule == null)
            throw new InvalidOperationException("Course does not have a schedule.");
        
        var evlIds = courseDto.Evls.Select(evl => evl.Id).ToList();
        var allCourseComponents = courseComponentService.GetAllCourseComponentByEvlIds(evlIds);

        return allCourseComponents.All(
            courseComponent => courseDto.Schedule.ScheduleLines.Any(sl => sl.CourseComponentId == courseComponent.Id)
        );
    }

    public bool HasCourseValidOrder(CourseDto courseDto)
    {
        if (courseDto.Schedule == null)
            throw new InvalidOperationException("Course does not have a schedule.");

        return courseDto.Evls.All(evl => EvlHasValidOrder(evl, courseDto.Schedule));
    }

    private bool EvlHasValidOrder(EvlDto evl, ScheduleDto schedule)
    {
        var courseComponents = courseComponentService.GetAllCourseComponentsByEvlId(evl.Id);

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