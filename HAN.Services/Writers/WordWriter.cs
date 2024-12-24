using HAN.Services.Interfaces;

namespace HAN.Services.Writers;

public class WordWriter : IFileTypeWriter
{
    public void WriteContent(string fileName, string content)
    {
        Console.WriteLine($"Writing content to Word file: {fileName}");
        System.IO.File.WriteAllText(fileName, content);
    }
}