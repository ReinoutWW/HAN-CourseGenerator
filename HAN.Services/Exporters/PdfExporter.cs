using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using HAN.Services.DTOs;
using QuestPDF.Helpers;

namespace HAN.Services.Exporters
{
    public class PdfExporter : FileExporter
    {
        protected override FileDto WriteContent(FileDto fileDto)
        {
            if (fileDto == null) throw new ArgumentNullException(nameof(fileDto), "FileDto cannot be null.");
            if (string.IsNullOrWhiteSpace(fileDto.Name)) throw new ArgumentException("File name cannot be empty.", nameof(fileDto.Name));
            if (string.IsNullOrWhiteSpace(fileDto.Content)) throw new ArgumentException("File content cannot be empty.", nameof(fileDto.Content));

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
}