using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using HAN.Services.DTOs;
using QuestPDF.Helpers;

namespace HAN.Services.Exporters;

public class PdfExporter : FileExporter
{
    public override FileDto Export(FileDto fileDto)
    {
        ValidateFile(fileDto);

        var fileName = fileDto.Name.EndsWith(".pdf") ? fileDto.Name : $"{fileDto.Name}.pdf";
        var filePath = GetExportFilePath(fileName);

        Console.WriteLine($"Generating PDF file at: {filePath}");
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Content().Column(column =>
                {
                    column.Item().Text($"Title: {fileDto.Name}")
                        .FontSize(20)
                        .Bold();
                    column.Item().Text(fileDto.Content).FontSize(12);
                });
            });
        }).GeneratePdf(filePath);
        
        return fileDto;
    }
}
