using System.ComponentModel.DataAnnotations;

namespace HAN.Services.DTOs;

public class CreateEvlDto
{
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
}