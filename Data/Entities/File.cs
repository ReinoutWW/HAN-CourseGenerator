using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HAN.Data.Entities;

public class File : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;
}