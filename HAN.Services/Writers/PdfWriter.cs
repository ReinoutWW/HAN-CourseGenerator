using HAN.Services.Interfaces;

namespace HAN.Services.Writers;

public class PdfWriter : IFileTypeWriter
{
    public void WriteContent(string fileName, string content)
    {
        Console.WriteLine($"Writing content to PDF file: {fileName}");
        System.IO.File.WriteAllText(fileName, content);
    }
}