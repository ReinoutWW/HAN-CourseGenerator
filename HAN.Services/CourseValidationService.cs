using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Services.Validation;

namespace HAN.Services;

public class CourseValidationService : ICourseValidationService
{
    private readonly ICourseService _courseService;
    private readonly ICourseCompletenessValidator _completenessValidator;
    private readonly ICourseOrderValidator _orderValidator;

    public CourseValidationService(
        ICourseService courseService,
        ICourseCompletenessValidator completenessValidator,
        ICourseOrderValidator orderValidator)
    {
        _courseService = courseService;
        _completenessValidator = completenessValidator;
        _orderValidator = orderValidator;
    }

    public CourseValidationResult ValidateCourse(int courseId)
    {
        var course = _courseService.GetCourseById(courseId);
        return ValidateCourseInternal(course);
    }

    public CourseValidationResult ValidateCourse(CourseDto courseDto)
    {
        return ValidateCourseInternal(courseDto);
    }

    public CourseValidationResult IsCourseComplete(CourseDto courseDto)
    {
        return _completenessValidator.Validate(courseDto);
    }

    public CourseValidationResult HasCourseValidOrder(CourseDto courseDto)
    {
        return _orderValidator.Validate(courseDto);
    }

    private CourseValidationResult ValidateCourseInternal(CourseDto courseDto)
    {
        if (courseDto.Schedule == null)
        {
            return CreateValidationResultWithError("Course does not have a schedule.", ErrorCategory.Missing, "Schedule");
        }

        var completenessResult = IsCourseComplete(courseDto);
        var orderResult = HasCourseValidOrder(courseDto);

        var errors = completenessResult.Errors.Concat(orderResult.Errors).ToList();

        return new CourseValidationResult
        {
            IsValid = completenessResult.IsValid && orderResult.IsValid,
            Message = completenessResult.IsValid && orderResult.IsValid
                ? "Course validation succeeded."
                : "Course validation failed.",
            Errors = errors
        };
    }

    private CourseValidationResult CreateValidationResultWithError(string message, ErrorCategory category, string detail)
    {
        return new CourseValidationResult
        {
            IsValid = false,
            Message = message,
            Errors = new List<CourseValidationError>
            {
                new CourseValidationError
                {
                    ErrorCategory = category,
                    Message = detail
                }
            }
        };
    }
}