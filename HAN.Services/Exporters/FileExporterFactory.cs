namespace HAN.Services.Exporters;

public enum ExporterType {
    Markdown,
    Pdf,
    Word
}

public class FileExporterFactory
{
    public static FileExporter GetExporter(ExporterType type)
    {
        return type switch
        {
            ExporterType.Markdown => new MarkdownExporter(),
            ExporterType.Pdf => new PdfExporter(),
            ExporterType.Word => new WordExporter(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Exporter type not supported.")
        };
    }
}