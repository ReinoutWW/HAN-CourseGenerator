using HAN.Services.Interfaces;

namespace HAN.Services.Writers;

public class MarkdownWriter : IFileTypeWriter
{
    public void WriteContent(string fileName, string content)
    {
        Console.WriteLine($"Writing content to Markdown file: {fileName}");
        System.IO.File.WriteAllText(fileName, content);
    }
}