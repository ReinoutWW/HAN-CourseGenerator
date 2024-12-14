using HAN.Domain.Entities;

namespace HAN.Services.DTOs;

public class CourseResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IEnumerable<Evl> Evls { get; set; } = new List<Evl>();
}