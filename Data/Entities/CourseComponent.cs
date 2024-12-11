using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HAN.Data.Entities;

public class CourseComponent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required.")] 
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")] 
    [MinLength(5, ErrorMessage = "Name cannot exceed 5 characters.")]
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(450, ErrorMessage = "Description cannot exceed 450 characters.")] 
    [DataType(DataType.Text)]
    public string Description { get; set; } = string.Empty;

    public List<Evl> Evls { get; set; } = [];
    public List<File> Files { get; set; } = [];
}