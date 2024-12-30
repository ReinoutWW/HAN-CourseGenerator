using HAN.Services.DTOs;
using HAN.Services.Interfaces;

namespace HAN.Services.Validation;

public class CourseCompletenessValidator(CourseComponentService courseComponentService) : ICourseCompletenessValidator
{
    public CourseValidationResult Validate(CourseDto courseDto)
    {
        var result = new CourseValidationResult
        {
            IsValid = true,
            Message = "Course completeness validation succeeded."
        };

        var evlIds = courseDto.Evls.Select(evl => evl.Id).ToList();
        var allCourseComponents = courseComponentService.GetAllCourseComponentByEvlIds(evlIds);

        foreach (var component in allCourseComponents)
        {
            if (!courseDto.Schedule.ScheduleLines.Any(sl => sl.CourseComponentId == component.Id))
            {
                result.IsValid = false;
                var evlNames = string.Join(", ", component.Evls.Select(evl => evl.Name));
                result.Errors.Add(new CourseValidationError
                {
                    ErrorCategory = ErrorCategory.Missing,
                    Message = $"{evlNames} : {component.Name}",
                    CourseComponentId = component.Id
                });
            }
        }

        if (!result.IsValid)
        {
            result.Message = "Course is incomplete. Missing components.";
        }

        return result;
    }
}