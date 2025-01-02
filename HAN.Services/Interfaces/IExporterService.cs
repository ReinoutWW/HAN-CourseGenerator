using HAN.Services.DTOs;

namespace HAN.Services.Interfaces;

public interface IExporterService
{

    public void ExportToMarkdown(FileDto file) {}
    public void ExportToWord(FileDto file) {}
    public void ExportToPdf(FileDto file) {}
    
}