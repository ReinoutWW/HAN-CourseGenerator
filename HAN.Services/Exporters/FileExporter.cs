using System.ComponentModel.DataAnnotations;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using ValidationException = HAN.Services.Exceptions.ValidationException;

namespace HAN.Services.Exporters;

public abstract class FileExporter()
{
    public const string ExportDirectory = "Downloads";
    
    public FileDto Export(FileDto fileDto)
    {
        ValidateFile(fileDto);
        return WriteContent(fileDto);
    }

    private static void ValidateFile(FileDto fileDto)
    {
        var validationContext = new ValidationContext(fileDto);
        var validationResults = new System.Collections.Generic.List<ValidationResult>();

        if (!Validator.TryValidateObject(fileDto, validationContext, validationResults, true))
        {
            throw new ValidationException($"FileDto validation failed: ", validationResults);
        }
    }
    
    protected static string GetExportFilePath(string fileName)
    {
        string exportDirectory = Path.Combine(Directory.GetCurrentDirectory(), ExportDirectory);
        if (!Directory.Exists(exportDirectory))
        {
            Directory.CreateDirectory(exportDirectory);
        }
        
        return Path.Combine(exportDirectory, fileName);
    }

    protected abstract FileDto WriteContent(FileDto fileDto);
}