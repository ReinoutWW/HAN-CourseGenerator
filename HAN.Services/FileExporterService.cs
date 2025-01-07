using AutoMapper;
using HAN.Repositories.Interfaces;
using HAN.Services.Attributes;
using HAN.Services.DTOs;
using HAN.Services.Exporters;
using HAN.Services.Interfaces;
using HAN.Services.Validation;
using File = HAN.Data.Entities.File;

namespace HAN.Services;

public class FileExporterService : IExporterService
{
    MarkdownExporter _markdownExporter;
    PdfExporter _pdfExporter;
    WordExporter _wordExporter;
    
    public FileExporterService()
    {
        _markdownExporter = new MarkdownExporter();
        _pdfExporter = new PdfExporter();
        _wordExporter = new WordExporter();
    }

    public void ExportToMarkdown(FileDto file)
    {
        Console.WriteLine($"Writing file to: {Path.Combine(Directory.GetCurrentDirectory(), file.Name)}.md");
        _markdownExporter.Export(file);
    }

    public void ExportToWord(FileDto file)
    {
        _wordExporter.Export(file);
    }

    public void ExportToPdf(FileDto file)
    {
        _pdfExporter.Export(file);
    }
}