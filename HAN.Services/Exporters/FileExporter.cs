using HAN.Services.DTOs;

namespace HAN.Services.Exporter;

public abstract class FileExporter
{
    public FileDto File { get; set; }

    protected FileExporter(FileDto file)
    {
        File = file;
    }

    // Abstract method to be implemented by derived classes
    public abstract void Export(string content);
}
