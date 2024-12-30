using HAN.Services.DTOs;
using HAN.Services.Validation;

namespace HAN.Services.Interfaces;

public interface ICourseValidationService
{
    public CourseValidationResult ValidateCourse(int courseId);
    public CourseValidationResult ValidateCourse(CourseDto courseDto);
    public CourseValidationResult IsCourseComplete(CourseDto courseDto);
    public CourseValidationResult HasCourseValidOrder(CourseDto courseDto);
}