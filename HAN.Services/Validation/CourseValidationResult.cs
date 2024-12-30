using System.ComponentModel;

namespace HAN.Services.Validation;

public class CourseValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public List<CourseValidationError> Errors { get; set; } = [];
}

public class CourseValidationError
{
    public ErrorCategory ErrorCategory { get; set; } = ErrorCategory.Invalid;
    public string Message { get; set; }
}

public enum ErrorCategory
{
    [Description("Missing")]
    Missing,
    [Description("Invalid")]
    Invalid,
    [Description("Out of order")]
    OutOfOrder
}