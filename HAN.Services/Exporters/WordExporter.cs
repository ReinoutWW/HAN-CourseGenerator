using System;
using System.IO;
using HAN.Services.DTOs;

namespace HAN.Services.Exporters
{
    public class WordExporter : FileExporter
    {
        protected override void WriteContent(FileDto fileDto)
        {
            if (fileDto == null) throw new ArgumentNullException(nameof(fileDto), "FileDto cannot be null.");
            if (string.IsNullOrWhiteSpace(fileDto.Name)) throw new ArgumentException("File name cannot be empty.", nameof(fileDto.Name));
            if (string.IsNullOrWhiteSpace(fileDto.Content)) throw new ArgumentException("File content cannot be empty.", nameof(fileDto.Content));
            
            string fileName = fileDto.Name.EndsWith(".docx") ? fileDto.Name : $"{fileDto.Name}.docx";

            Console.WriteLine($"Writing Word content to {fileName}...");

            try
            {
                using (var writer = new StreamWriter(fileName))
                {
                    writer.WriteLine($"Title: {fileDto.Name}");
                    writer.WriteLine();
                    writer.WriteLine(fileDto.Content);
                }

                Console.WriteLine("Word content successfully written.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing the Word file: {ex.Message}");
                throw;
            }
        }
    }
}