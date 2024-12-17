using HAN.Domain.Validation;

namespace HAN.Domain.Entities;

public class Course
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public ICollection<Evl> Evls { get; set; } = new List<Evl>();
    public CourseSchedule Schedule { get; set; } = new CourseSchedule();
    
    public ValidationResult Validate()
    {
        if (Evls.Count == 0)
        {
            return new ValidationResult(false, "A course must have at least one EVL.");
        }

        foreach (var evl in Evls)
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