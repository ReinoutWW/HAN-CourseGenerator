using HAN.Services.DTOs;
using HAN.Services.Validation;

namespace HAN.Services.Interfaces;

public interface ICourseOrderValidator
{
    CourseValidationResult Validate(CourseDto courseDto);
}
