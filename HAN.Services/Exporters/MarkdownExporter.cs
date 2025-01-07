using HAN.Services.DTOs;

namespace HAN.Services.Exporters;

public class MarkdownExporter() : FileExporter()
{
    protected override void WriteContent(FileDto fileDto)
    {
        if (fileDto == null) throw new ArgumentNullException(nameof(fileDto), "FileDto cannot be null.");
        if (string.IsNullOrWhiteSpace(fileDto.Name)) throw new ArgumentException("File name cannot be empty.", nameof(fileDto.Name));
        if (string.IsNullOrWhiteSpace(fileDto.Content)) throw new ArgumentException("File content cannot be empty.", nameof(fileDto.Content));
        
        string fileName = fileDto.Name.EndsWith(".md") ? fileDto.Name : $"{fileDto.Name}.md";
        
        string markdownContent = TransformToMarkdown(fileDto);
        
        Console.WriteLine($"Writing Markdown content to {fileName}...");
        File.WriteAllText(fileName, markdownContent);

        Console.WriteLine("Markdown content successfully written.");
    }
    
    private static string TransformToMarkdown(FileDto fileDto)
    {
        return $"# {fileDto.Name}\n\n{fileDto.Content}";
    }
}