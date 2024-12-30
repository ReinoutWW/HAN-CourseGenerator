using HAN.Services.DTOs;
using HAN.Services.Validation;

namespace HAN.Services.Interfaces;

public interface ICourseCompletenessValidator
{
    CourseValidationResult Validate(CourseDto courseDto);
}