using System.ComponentModel.DataAnnotations;

namespace HAN.Domain.Entities;

public class Evl
{
    [Required(ErrorMessage = "Name is required.")] 
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")] 
    [MinLength(5, ErrorMessage = "Name should be at least 5 characters.")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required.")] 
    [StringLength(450, ErrorMessage = "Description cannot exceed 450 characters.")] 
    [MinLength(5, ErrorMessage = "Description should be at least 5 characters.")]
    public string? Description { get; set; }
}