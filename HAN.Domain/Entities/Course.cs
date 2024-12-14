using HAN.Domain.Validation;

namespace HAN.Domain.Entities;

public class Course
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public ICollection<Evl> EVLs { get; set; } = new List<Evl>();
    
    public ValidationResult Validate()
    {
        if (EVLs.Count == 0)
        {
            return new ValidationResult(false, "A course must have at least one EVL.");
        }

        foreach (var evl in EVLs)
        {
            var evlValidation = evl.Validate();
            if (!evlValidation.IsValid)
            {
                return evlValidation;
            }
        }

        return ValidationResult.Valid();
    }
}