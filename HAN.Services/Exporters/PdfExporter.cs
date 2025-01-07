using System;
using System.IO;
using HAN.Services.DTOs;

namespace HAN.Services.Exporters
{
    public class PdfExporter : FileExporter
    {
        protected override void WriteContent(FileDto fileDto)
        {
            if (fileDto == null) throw new ArgumentNullException(nameof(fileDto), "FileDto cannot be null.");
            if (string.IsNullOrWhiteSpace(fileDto.Name)) throw new ArgumentException("File name cannot be empty.", nameof(fileDto.Name));
            if (string.IsNullOrWhiteSpace(fileDto.Content)) throw new ArgumentException("File content cannot be empty.", nameof(fileDto.Content));
            
            string fileName = fileDto.Name.EndsWith(".pdf") ? fileDto.Name : $"{fileDto.Name}.pdf";

            Console.WriteLine($"Writing PDF content to {fileName}...");

            try
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine($"Title: {fileDto.Name}");
                    writer.WriteLine();
                    writer.WriteLine(fileDto.Content);
                }

                Console.WriteLine("PDF content successfully written.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing the PDF: {ex.Message}");
                throw;
            }
        }
    }
}