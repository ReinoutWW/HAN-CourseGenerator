
namespace HAN.Services.DTOs.CourseComponents;

public class CourseComponentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    public List<EvlDto>? Evls { get; set; } = [];
    public List<FileDto>? Files { get; set; } = [];
}