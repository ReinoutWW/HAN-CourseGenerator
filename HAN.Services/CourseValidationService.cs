using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using HAN.Services.Validation;

namespace HAN.Services;

public class CourseValidationService(
    ICourseService courseService,
    CourseComponentService courseComponentService
) : ICourseValidationService
{
    public CourseValidationResult ValidateCourse(int courseId)
    {
        var course = courseService.GetCourseById(courseId);

        if (course.Schedule == null)
        {
            return new CourseValidationResult
            {
                IsValid = false,
                Message = "Course does not have a schedule.",
                Errors = new List<CourseValidationError>
                {
                    new CourseValidationError
                    {
                        ErrorCategory = ErrorCategory.Missing,
                        Message = "Schedule"
                    }
                }
            };
        }

        var completenessResult = IsCourseComplete(course);
        var orderResult = HasCourseValidOrder(course);

        var errors = new List<CourseValidationError>();
        errors.AddRange(completenessResult.Errors);
        errors.AddRange(orderResult.Errors);

        return new CourseValidationResult
        {
            IsValid = completenessResult.IsValid && orderResult.IsValid,
            Message = completenessResult.IsValid && orderResult.IsValid
                ? "Course validation succeeded."
                : "Course validation failed.",
            Errors = errors
        };
    }

    public CourseValidationResult ValidateCourse(CourseDto courseDto)
    {
        if (courseDto.Schedule == null)
        {
            return new CourseValidationResult
            {
                IsValid = false,
                Message = "Course does not have a schedule.",
                Errors = new List<CourseValidationError>
                {
                    new CourseValidationError
                    {
                        ErrorCategory = ErrorCategory.Missing,
                        Message = "Schedule"
                    }
                }
            };
        }

        var completenessResult = IsCourseComplete(courseDto);
        var orderResult = HasCourseValidOrder(courseDto);

        var errors = new List<CourseValidationError>();
        errors.AddRange(completenessResult.Errors);
        errors.AddRange(orderResult.Errors);

        return new CourseValidationResult
        {
            IsValid = completenessResult.IsValid && orderResult.IsValid,
            Message = completenessResult.IsValid && orderResult.IsValid
                ? "Course validation succeeded."
                : "Course validation failed.",
            Errors = errors
        };
    }

    public CourseValidationResult IsCourseComplete(CourseDto courseDto)
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
                result.Errors.Add(new CourseValidationError
                {
                    ErrorCategory = ErrorCategory.Missing,
                    Message = $"CourseComponent: {component.Name}"
                });
            }
        }

        if (!result.IsValid)
        {
            result.Message = "Course is incomplete. Missing components.";
        }

        return result;
    }

    public CourseValidationResult HasCourseValidOrder(CourseDto courseDto)
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
                var scheduleLine = courseDto.Schedule.ScheduleLines
                    .FirstOrDefault(sl => sl.CourseComponentId == component.Id);

                if (scheduleLine == null)
                    continue;

                if (component is ExamDto)
                {
                    var lessonsAfterExam = courseDto.Schedule.ScheduleLines
                        .Where(sl => sl.CourseComponent is LessonDto)
                        .Where(lesson => lesson.WeekSequenceNumber > scheduleLine.WeekSequenceNumber)
                        .ToList();

                    if (lessonsAfterExam.Any())
                    {
                        result.IsValid = false;
                        result.Errors.Add(new CourseValidationError
                        {
                            ErrorCategory = ErrorCategory.OutOfOrder,
                            Message = $"Exam: {component.Name}"
                        });

                        foreach (var lesson in lessonsAfterExam)
                        {
                            result.Errors.Add(new CourseValidationError
                            {
                                ErrorCategory = ErrorCategory.OutOfOrder,
                                Message = $"Lesson: {lesson.CourseComponent.Name}"
                            });
                        }
                    }
                }
            }
        }

        if (!result.IsValid)
        {
            result.Message = "Course has invalid order. Issues found.";
        }

        return result;
    }
}
