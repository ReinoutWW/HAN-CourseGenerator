using System.ComponentModel.DataAnnotations;

namespace HAN.Data;

public static class EntityValidator
{
    public static List<ValidationException> GetValidationExceptionsForEntities(IEnumerable<object> entities)
    {
        var validationExceptions = new List<ValidationException>();

        foreach (var entity in entities)
        {
            validationExceptions.AddRange(
                AddValidationExceptionsForEntity(entity)
            );
        }

        return validationExceptions;
    }

    public static List<ValidationException> AddValidationExceptionsForEntity(object entity)
    {
        var validationExceptions = new List<ValidationException>();
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(entity);
        
        if (!Validator.TryValidateObject(entity, context, validationResults, true))
        {
            foreach (var validationResult in validationResults)
            {
                validationExceptions.Add(new ValidationException(validationResult.ErrorMessage));
            }
        }
        
        return validationExceptions;
    }
}