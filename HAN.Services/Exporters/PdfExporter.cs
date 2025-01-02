using HAN.Services.Writers;

namespace HAN.Services.Exporters;

public class PdfExporter() : FileExporter(new PdfWriter())
{
    protected override void PrepareFile()
    {
        Console.WriteLine("Preparing PDF file...");
    }

    protected override void FinalizeFile()
    {
        Console.WriteLine("PDF export finalized.");
    }
}