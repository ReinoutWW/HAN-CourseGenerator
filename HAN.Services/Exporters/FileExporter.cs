using HAN.Services.DTOs;
using HAN.Services.Interfaces;

namespace HAN.Services.Exporters;

public abstract class FileExporter : IExporterService
{
    protected FileDto File { get; set; }

    protected FileExporter(FileDto file)
    {
        File = file;
    }
    
    public abstract void Export(string content);
}
