
using System.ComponentModel.DataAnnotations;

namespace HAN.Services.Exceptions;

public class ValidationException(string message, List<ValidationResult> validationErrors) : Exception(message)
{
    public List<ValidationResult> ValidationErrors { get; } = validationErrors;

    public override string ToString()
    {
        return $"{base.ToString()}\nValidation Errors: {string.Join(", ", ValidationErrors)}";
    }
    
}