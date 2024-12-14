
using HAN.Domain.Validation;

namespace HAN.Domain.Entities;

public class Evl
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public ValidationResult Validate()
    {
        if (Lessons.Count == 0)
        {
            return new ValidationResult(false, "An EVL must have at least one Lesson.");
        }
        if (Exams.Count == 0)
        {
            return new ValidationResult(false, "An EVL must have at least one Exam.");
        }
        return ValidationResult.Valid();
    }
}