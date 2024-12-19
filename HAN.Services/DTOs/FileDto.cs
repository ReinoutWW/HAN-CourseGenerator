using System.ComponentModel.DataAnnotations;

namespace HAN.Services.DTOs;

public class FileDto
{
    public int Id { get; set; }
    
    [Required]
    public string FileName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}