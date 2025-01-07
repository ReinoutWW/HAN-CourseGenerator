using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class FileExporterServiceTests : TestBase
{
    private readonly IFileService _fileService;
    private readonly IExporterService _exporterService;

    public FileExporterServiceTests()
    {
        _fileService = ServiceProvider.GetRequiredService<IFileService>();
        _exporterService = ServiceProvider.GetRequiredService<IExporterService>();
    }
    
    private FileDto file()
    {
        return new FileDto
        {
            Name = "export",
            Extension = "txt",
            Content = "This content is used for testing purposes."
        };
    }
    
    [Fact]
    public void Export_CreatesMarkdownFileWithCorrectContent()
    {
        // Arrange
        var directory = Path.Combine(Directory.GetCurrentDirectory());

        var fileDto = file();
        var expectedFileName = Path.Combine(directory, "export.md");
        var expectedContent = "# export\n\nThis content is used for testing purposes.";
        
        // Act
        _exporterService.ExportToMarkdown(fileDto);

        // Assert
        Assert.True(File.Exists(expectedFileName), $"Markdown file was not created at {expectedFileName}.");
        
        var actualContent = File.ReadAllText(expectedFileName);
        Assert.Equal(expectedContent, actualContent);
        
        // Cleanup
        if (File.Exists(expectedFileName))
        {
            File.Delete(expectedFileName);
        }
    }
    
    [Fact]
    public void Export_CreatesPdfFileWithCorrectContent()
    {
        // Arrange
        var directory = Path.Combine(Directory.GetCurrentDirectory());

        var fileDto = file();
        var expectedFileName = Path.Combine(directory, "export.pdf");
        var expectedContent = $"Title: export\n\nThis content is used for testing purposes.";

        // Act
        _exporterService.ExportToPdf(fileDto);

        // Assert
        Assert.True(File.Exists(expectedFileName), $"PDF file was not created at {expectedFileName}.");

        var actualContent = File.ReadAllText(expectedFileName).Replace("\r\n", "\n").TrimEnd();
        Assert.Equal(expectedContent, actualContent);

        // Cleanup
        if (File.Exists(expectedFileName))
        {
            File.Delete(expectedFileName);
        }
    }

}