using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using QuestPDF.Infrastructure;

namespace HAN.Tests.Services;

public class FileExporterServiceTests : TestBase
{
    private readonly IFileService _fileService;
    private readonly IExporterService _exporterService;

    public FileExporterServiceTests()
    {
        _fileService = ServiceProvider.GetRequiredService<IFileService>();
        _exporterService = ServiceProvider.GetRequiredService<IExporterService>();
        
        // Configure QuestPDF license for tests
        QuestPDF.Settings.License = LicenseType.Community;
    }
    
    private FileDto file()
    {
        return new FileDto
        {
            Name = "export",
            Content = "This content is used for testing purposes."
        };
    }

    private string GetExportPath(string fileName)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "Downloads", fileName);
    }

    [Fact]
    public void Export_CreatesMarkdownFileWithCorrectContent()
    {
        // Arrange
        var fileDto = file();
        var expectedFileName = GetExportPath("export.md");
        var expectedContent = "# export\n\nThis content is used for testing purposes.";

        try
        {
            // Act
            _exporterService.ExportToMarkdown(fileDto);

            // Assert
            Assert.True(File.Exists(expectedFileName), $"Markdown file was not created at {expectedFileName}.");
            
            var actualContent = File.ReadAllText(expectedFileName);
            Assert.Equal(expectedContent, actualContent);
        }
        finally
        {
            // Cleanup
            if (File.Exists(expectedFileName))
            {
                File.Delete(expectedFileName);
            }
        }
    }
    
    [Fact]
    public void Export_CreatesWordFile()
    {
        // Arrange
        var fileDto = file();
        var expectedFileName = GetExportPath("export.docx");

        try
        {
            // Act
            _exporterService.ExportToWord(fileDto);

            // Assert
            Assert.True(File.Exists(expectedFileName), $"Word file was not created at {expectedFileName}.");

            // Controleer of het bestand niet leeg is
            using (var fileStream = File.OpenRead(expectedFileName))
            {
                Assert.True(fileStream.Length > 0, "Word file is empty.");
            }
        }
        finally
        {
            // Cleanup
            if (File.Exists(expectedFileName))
            {
                File.Delete(expectedFileName);
            }
        }
    }
    
    [Fact]
    public void Export_CreatesPdfFile()
    {
        // Arrange
        var fileDto = file();
        var expectedFileName = GetExportPath("export.pdf");

        try
        {
            // Act
            _exporterService.ExportToPdf(fileDto);

            // Assert
            Assert.True(File.Exists(expectedFileName), $"PDF file was not created at {expectedFileName}.");
            
            using (var fileStream = File.OpenRead(expectedFileName))
            {
                Assert.True(fileStream.Length > 0, "PDF file is empty.");
            }
        }
        finally
        {
            // Cleanup
            if (File.Exists(expectedFileName))
            {
                File.Delete(expectedFileName);
            }
        }
    }
}
