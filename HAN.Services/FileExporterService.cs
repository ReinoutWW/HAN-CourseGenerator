using HAN.Services.DTOs;
using HAN.Services.Exporters;
using HAN.Services.Interfaces;

namespace HAN.Services;

public class FileExporterService : IExporterService
{
    public void ExportToFile(FileDto file, ExporterType type)
    {
        Console.WriteLine($"Downloading file: {file.Name}.pdf");
        FileExporterFactory.GetExporter(type).Export(file);
    }
}