using FluentValidation;
using HAN.Services.DTOs;

namespace HAN.Services.Validators;

public class CourseValidator : AbstractValidator<CreateCourseDto>
{
    public CourseValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(5, 100).WithMessage("Name must be between 2 and 100 characters");
        
        RuleFor(c => c.Description)
            .Length(0, 450).WithMessage("Description must be between 2 and 100 characters");
    }
}