using HAN.Services.DTOs;

namespace HAN.Services.Exporters;

public class MarkdownExporter() : FileExporter()
{
    protected override FileDto WriteContent(FileDto fileDto)
    {
        if (fileDto == null) throw new ArgumentNullException(nameof(fileDto), "FileDto cannot be null.");
        if (string.IsNullOrWhiteSpace(fileDto.Name)) throw new ArgumentException("File name cannot be empty.", nameof(fileDto.Name));
        if (string.IsNullOrWhiteSpace(fileDto.Content)) throw new ArgumentException("File content cannot be empty.", nameof(fileDto.Content));
        
        var fileName = fileDto.Name.EndsWith(".md") ? fileDto.Name : $"{fileDto.Name}.md";
        var markdownContent = TransformToMarkdown(fileDto);
        var filePath = GetExportFilePath(fileName);
        
        Console.WriteLine($"Generating Markdown file at: {filePath}");
        File.WriteAllText(filePath, markdownContent);
        return fileDto;
    }
    
    private static string TransformToMarkdown(FileDto fileDto)
    {
        return $"# {fileDto.Name}\n\n{fileDto.Content}";
    }
}