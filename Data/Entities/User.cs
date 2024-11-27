using System.ComponentModel.DataAnnotations;

namespace HAN.Data.Entities;

public class User
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    [Required] 
    public string Name { get; set; } = string.Empty;
}