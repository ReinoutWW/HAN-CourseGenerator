using System.ComponentModel.DataAnnotations;
using ValidationException = HAN.Services.Exceptions.ValidationException;

namespace HAN.Services.Validation;

/// <summary>
/// Validation service for DataAnnotations
/// </summary>
public class ValidationService : IValidationService
{
    public void Validate<T>(T entity)
    {
        if (Equals(entity, default(T))) throw new ArgumentNullException(nameof(entity));
        
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(entity!);

        if (!Validator.TryValidateObject(entity!, context, validationResults, true))
        {
            throw new ValidationException($"Validation failed for {typeof(T).Name}", validationResults);
        }
    }
}