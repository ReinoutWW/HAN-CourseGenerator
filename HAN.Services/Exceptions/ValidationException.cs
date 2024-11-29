using FluentValidation.Results;

namespace HAN.Services.Exceptions;

public class ValidationException(string message, List<ValidationFailure> validationErrors) : Exception(message)
{
    public List<ValidationFailure> ValidationErrors { get; } = validationErrors ?? [];

    public override string ToString()
    {
        return $"{base.ToString()}\nValidation Errors: {string.Join(", ", ValidationErrors)}";
    }
    
}