namespace HAN.Domain.Validation;
    
public class ValidationResult
{
    public bool IsValid { get; private set; }
    public string ErrorMessage { get; private set; }

    public ValidationResult(bool isValid, string errorMessage = "")
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    public static ValidationResult Valid() => new ValidationResult(true);
    public static ValidationResult Invalid(string errorMessage) => new ValidationResult(false, errorMessage);
}