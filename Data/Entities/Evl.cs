using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HAN.Data.Entities.CourseComponents;

namespace HAN.Data.Entities;

public class Evl : BaseEntity
{
    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Text)]
    public string? Description { get; set; }
    
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}