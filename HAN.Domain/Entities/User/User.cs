using System.ComponentModel.DataAnnotations;

namespace HAN.Domain.Entities.User;

public class User
{
    [Required(ErrorMessage = "Name is required.")] 
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")] 
    [MinLength(5, ErrorMessage = "Name cannot exceed 5 characters.")]
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")] 
    [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
    [MinLength(5, ErrorMessage = "Name cannot exceed 5 characters.")]
    [DataType(DataType.Text)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    [MinLength(8, ErrorMessage = "Name cannot exceed 8 characters.")]
    [DataType(DataType.Text)]
    public string Password { get; set; } = string.Empty;
}