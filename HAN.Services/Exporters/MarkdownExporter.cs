using HAN.Services.DTOs;

namespace HAN.Services.Exporter;

public class MarkdownExporter : FileExporter
{
    public MarkdownExporter(FileDto file) : base(file) { }

    public override void Export(string content)
    {
        Console.WriteLine($"Exporting to Markdown file: {File.FileName}");
        // Add logic to write content to a .md file
        System.IO.File.WriteAllText(File.FileName, content);
        Console.WriteLine("Markdown export completed.");
    }
    
    public string ToMarkdown() 
    {
        return "this should be a markdown string";
    }
}