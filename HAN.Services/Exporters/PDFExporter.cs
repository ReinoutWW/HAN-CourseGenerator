using HAN.Services.DTOs;

namespace HAN.Services.Exporters;

public class PDFExporter : FileExporter
{
    public PDFExporter(FileDto file) : base(file) { }

    public override void Export(string content)
    {
        Console.WriteLine($"Exporting to PDF file: {File.Name}");
        // Add logic to write content to a .pdf file (using a library like iTextSharp or PDFSharp)
        // Placeholder code for now:
        System.IO.File.WriteAllText(File.Name, content); // Temporary logic
        Console.WriteLine("PDF export completed.");
    }
    
    public string ToPDF() 
    {
        return "this should be a PDF string";
    }
}