using System.ComponentModel.DataAnnotations;

namespace HAN.Services.DTOs;

public class FileDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required.")] 
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")] 
    [MinLength(2, ErrorMessage = "Name should be at least 5 characters.")]
    public string FileName { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;
}