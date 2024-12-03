using System.ComponentModel.DataAnnotations;

namespace HAN.Data;

public static class EntityValidator
{
    public static List<ValidationException> GetValidationExceptionsForEntities(IEnumerable<object> entities)
    {
        var validationExceptions = new List<ValidationException>();
        foreach (var entity in entities)
        {
            validationExceptions.AddRange(AddValidationExceptionsForEntity(entity));
        }

        return validationExceptions;
    }

    private static List<ValidationException> AddValidationExceptionsForEntity(object entity)
    {
        var validationExceptions = new List<ValidationException>();
        var validationResults = new List<ValidationResult>();

        if (Validator.TryValidateObject(
                entity, 
                new ValidationContext(entity), 
                validationResults, true)
        ) return validationExceptions;
        
        validationExceptions.AddRange(
            validationResults.Select(
                validationResult => new ValidationException(validationResult.ErrorMessage)
            )
        );

        return validationExceptions;
    }
}