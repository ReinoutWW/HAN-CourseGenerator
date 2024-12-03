using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HAN.Data.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

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