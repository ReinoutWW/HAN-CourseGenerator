using HAN.Services.DTOs;
using HAN.Services.Writers;

namespace HAN.Services.Exporters;

public class MarkdownExporter() : FileExporter(new MarkdownWriter())
{
    protected override void PrepareFile()
    {
        Console.WriteLine("Preparing Markdown file...");
    }

    protected override void FinalizeFile()
    {
        Console.WriteLine("Markdown export finalized.");
    }
}