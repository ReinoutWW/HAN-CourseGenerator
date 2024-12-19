using HAN.Services.DTOs;

namespace HAN.Services.Exporters;

public class WordExporter : FileExporter
{
    public WordExporter(FileDto file) : base(file) { }

    public override void Export(string content)
    {
        Console.WriteLine($"Exporting to Word file: {File.Name}");
        // Add logic to write content to a .docx file (using a library like OpenXML SDK)
        // Placeholder code for now:
        System.IO.File.WriteAllText(File.Name, content); // Temporary logic
        Console.WriteLine("Word export completed.");
    }
    
    public string ToWord() 
    {
        return "this should be a Word string";
    }
}