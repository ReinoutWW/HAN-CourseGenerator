using HAN.Services.DTOs;

namespace HAN.Services.Exporters;

public class MarkdownExporter : FileExporter
{
    public MarkdownExporter(FileDto file) : base(file) { }

    public override void Export(string content)
    {
        Console.WriteLine($"Exporting to Markdown file: {File.Name}");
        // Add logic to write content to a .md file
        System.IO.File.WriteAllText(File.Name, content);
        Console.WriteLine("Markdown export completed.");
    }
    
    public string ToMarkdown() 
    {
        return "this should be a markdown string";
    }
}