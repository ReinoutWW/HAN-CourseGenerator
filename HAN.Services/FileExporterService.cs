using AutoMapper;
using HAN.Repositories.Interfaces;
using HAN.Services.Attributes;
using HAN.Services.DTOs;
using HAN.Services.Exporters;
using HAN.Services.Interfaces;
using HAN.Services.Validation;
using File = HAN.Data.Entities.File;

namespace HAN.Services;

public class FileExporterService(MarkdownExporter markdownExporter, PdfExporter pdfExporter, WordExporter wordExporter) : IExporterService
{
    
    public void ExportToMarkdown(FileDto file)
    {
        markdownExporter.Export(file);
    }
    
    public void ExportToWord(FileDto file)
    {
        wordExporter.Export(file);
    }
    
    public void ExportToPdf(FileDto file)
    {
        pdfExporter.Export(file);
    }
    
}