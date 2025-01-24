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
    public void ExportToFile(FileDto file, ExporterType type)
    {
        Console.WriteLine($"Downloading file: {file.Name}.pdf");
        FileExporterFactory.GetExporter(type).Export(file);
    }
}