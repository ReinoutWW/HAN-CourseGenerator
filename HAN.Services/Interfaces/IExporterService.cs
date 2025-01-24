using HAN.Services.DTOs;
using HAN.Services.Exporters;

namespace HAN.Services.Interfaces;

public interface IExporterService
{
    void ExportToFile(FileDto file, ExporterType type);
}