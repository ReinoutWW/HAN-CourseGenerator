using System.ComponentModel.DataAnnotations;

namespace HAN.Data.Entities;

public class User : BaseEntity
{
    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Text)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Text)]
    public string Password { get; set; } = string.Empty;
}