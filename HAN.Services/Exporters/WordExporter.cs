using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HAN.Services.DTOs;
using Document = System.Reflection.Metadata.Document;

namespace HAN.Services.Exporters
{
    public class WordExporter : FileExporter
    {
        protected override FileDto WriteContent(FileDto fileDto)
        {
            if (fileDto == null) throw new ArgumentNullException(nameof(fileDto), "FileDto cannot be null.");
            if (string.IsNullOrWhiteSpace(fileDto.Name)) throw new ArgumentException("File name cannot be empty.", nameof(fileDto.Name));
            if (string.IsNullOrWhiteSpace(fileDto.Content)) throw new ArgumentException("File content cannot be empty.", nameof(fileDto.Content));
    
            var fileName = fileDto.Name.EndsWith(".docx") ? fileDto.Name : $"{fileDto.Name}.docx";
            var filePath = GetExportFilePath(fileName);

            Console.WriteLine($"Generating Word file at: {filePath}");

            using (var wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                var body = mainPart.Document.AppendChild(new Body());
                var paragraph = body.AppendChild(new Paragraph());
                var run = paragraph.AppendChild(new Run());
                run.AppendChild(new Text(fileDto.Content));
            }

            return fileDto;
        }

    }
}
