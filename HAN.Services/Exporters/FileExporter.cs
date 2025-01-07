using System.ComponentModel.DataAnnotations;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using ValidationException = HAN.Services.Exceptions.ValidationException;

namespace HAN.Services.Exporters;

public abstract class FileExporter()
{
    public void Export(FileDto fileDto)
    {
        ValidateFile(fileDto);

        var fileName = fileDto.Name;
        Console.WriteLine($"Preparing to export to: {fileName}");
        PrepareFile();
        WriteContent(fileDto);
        FinalizeFile();
        Console.WriteLine($"Export completed for: {fileName}");
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

    protected virtual void PrepareFile()
    {
        Console.WriteLine("Default preparation before export...");
    }

    protected virtual void FinalizeFile()
    {
        Console.WriteLine("Default finalization after export...");
    }

    protected abstract void WriteContent(FileDto fileDto);
}