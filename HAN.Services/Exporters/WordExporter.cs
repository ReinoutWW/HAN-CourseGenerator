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
        public override FileDto Export(FileDto fileDto)
        {
            ValidateFile(fileDto);
    
            var fileName = fileDto.Name.EndsWith(".docx") ? fileDto.Name : $"{fileDto.Name}.docx";
            var filePath = GetExportFilePath(fileName);

            Console.WriteLine($"Generating Word file at: {filePath}");
            using var wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);
            var mainPart = wordDocument.AddMainDocumentPart();
            mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
            var body = mainPart.Document.AppendChild(new Body());
            var paragraph = body.AppendChild(new Paragraph());
            var run = paragraph.AppendChild(new Run());
            run.AppendChild(new Text(fileDto.Content));

            return fileDto;
        }

    }
}
