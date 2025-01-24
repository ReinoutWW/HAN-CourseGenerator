using HAN.Services.DTOs;

namespace HAN.Services.Exporters;

public class MarkdownExporter() : FileExporter()
{
    public override FileDto Export(FileDto fileDto)
    {
        ValidateFile(fileDto);
        
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