namespace HAN.Services.DTOs.CourseComponents;

public abstract class CourseComponent
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    public List<EvlDto>? Evls { get; set; } = [];
    public List<FileDto>? Files { get; set; } = [];
}