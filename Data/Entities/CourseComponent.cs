using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HAN.Data.Entities;

public class CourseComponent : BaseEntity
{
    [Required]
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;
    
    [DataType(DataType.Text)]
    public string Description { get; set; } = string.Empty;

    public List<Evl> Evls { get; set; } = [];
    public List<File> Files { get; set; } = [];
}