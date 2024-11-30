namespace HAN.Services.Validation;

public interface IValidationService
{
    void Validate<T>(T entity);
}