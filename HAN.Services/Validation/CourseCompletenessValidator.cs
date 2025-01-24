using HAN.Services.DTOs;
using HAN.Services.Interfaces;

namespace HAN.Services.Validation;

public class CourseCompletenessValidator(CourseComponentService courseComponentService) : ICourseCompletenessValidator
{
    private readonly CourseComponentService _courseComponentService = courseComponentService;

    public CourseValidationResult Validate(CourseDto courseDto)
    {
        var result = new CourseValidationResult
        {
            IsValid = true,
            Message = "Course completeness validation succeeded."
        };

        var evlIds = courseDto.Evls.Select(evl => evl.Id).ToList();
        var validComponents = _courseComponentService
            .GetAllCourseComponentByEvlIds(evlIds);

        var scheduledCourseComponentIds = courseDto.Schedule.ScheduleLines
            .Select(sl => sl.CourseComponentId)
            .ToHashSet();

        foreach (var component in validComponents)
        {
            var hasRemovedComponentFromSchedule = scheduledCourseComponentIds.Remove(component.Id);

            if (!hasRemovedComponentFromSchedule)
            {
                var evlNames = string.Join(", ", component.Evls.Select(e => e.Name));
                result.Errors.Add(new CourseValidationError
                {
                    ErrorCategory = ErrorCategory.Missing,
                    Message = $"{evlNames} : {component.Name}",
                    CourseComponentId = component.Id
                });
            }
        }

        result.Errors.AddRange(from extraId in scheduledCourseComponentIds
                               select new CourseValidationError
                               {
                                   ErrorCategory = ErrorCategory.ExtraComponent,
                                   Message = $"Component ID {extraId} is not part of any EVL.",
                                   CourseComponentId = extraId
                               });

        if (result.Errors.Any())
        {
            result.IsValid = false;
            result.Message = "Course completeness validation failed: missing or extra components.";
        }

        return result;
    }
}