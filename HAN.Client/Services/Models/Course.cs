using System.Text.Json.Serialization;

namespace HAN.Client.Services.Models;

public class Course
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}