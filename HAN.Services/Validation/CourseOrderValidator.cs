using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;

namespace HAN.Services.Validation;


public class CourseOrderValidator(CourseComponentService courseComponentService) : ICourseOrderValidator
{
    public CourseValidationResult Validate(CourseDto courseDto)
    {
        var result = new CourseValidationResult
        {
            IsValid = true,
            Message = "Course order validation succeeded."
        };

        foreach (var evl in courseDto.Evls)
        {
            var courseComponents = courseComponentService.GetAllCourseComponentsByEvlId(evl.Id);

            foreach (var component in courseComponents)
            {
                var scheduleLine = courseDto.Schedule.ScheduleLines.FirstOrDefault(sl => sl.CourseComponentId == component.Id);

                if (scheduleLine == null) continue;

                if (component is ExamDto && HasLessonsAfter(scheduleLine, courseDto))
                {
                    result.IsValid = false;
                    result.Errors.Add(new CourseValidationError
                    {
                        ErrorCategory = ErrorCategory.OutOfOrder,
                        Message = $"Exam: {component.Name}",
                        CourseComponentId = component.Id
                    });
                }
            }
        }

        if (!result.IsValid)
        {
            result.Message = "Course has invalid order. Issues found.";
        }

        return result;
    }

    private bool HasLessonsAfter(ScheduleLineDto scheduleLine, CourseDto courseDto)
    {
        return courseDto.Schedule.ScheduleLines
            .Where(sl => sl.CourseComponent is LessonDto)
            .Any(lesson => lesson.WeekSequenceNumber > scheduleLine.WeekSequenceNumber);
    }
}
