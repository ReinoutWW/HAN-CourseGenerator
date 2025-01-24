using System.ComponentModel.DataAnnotations;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using ValidationException = HAN.Services.Exceptions.ValidationException;

namespace HAN.Services.Exporters;

public abstract class FileExporter()
{
    public const string ExportDirectory = "Downloads";

    public abstract FileDto Export(FileDto fileDto);

    protected static void ValidateFile(FileDto fileDto)
    {
        var validationContext = new ValidationContext(fileDto);
        var validationResults = new System.Collections.Generic.List<ValidationResult>();

        if (!Validator.TryValidateObject(fileDto, validationContext, validationResults, true))
        {
            throw new ValidationException($"FileDto validation failed: ", validationResults);
        }
        
        if (fileDto == null) throw new ArgumentNullException(nameof(fileDto), "FileDto cannot be null.");
        if (string.IsNullOrWhiteSpace(fileDto.Name)) throw new ArgumentException("File name cannot be empty.", nameof(fileDto.Name));
        if (string.IsNullOrWhiteSpace(fileDto.Content)) throw new ArgumentException("File content cannot be empty.", nameof(fileDto.Content));
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
}