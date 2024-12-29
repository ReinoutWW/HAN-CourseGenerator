using HAN.Services.DTOs;

namespace HAN.Services.Interfaces;

public interface ICourseValidationService
{
    public bool ValidateCourse(int courseId);
    public bool ValidateCourse(CourseDto courseDto);
    public bool IsCourseComplete(CourseDto courseDto);
}