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
            Name = "Example",
            Content = "This content used for testing purposes."
        };
    }
    
    [Fact]
    public void Export_CreatesMarkdownFileWithCorrectContent()
    {
        var fileDto = file();
        var outputFileName = "TestFile.md";
        
        _exporterService.ExportToMarkdown(fileDto);
        
        Assert.True(File.Exists(outputFileName), "Markdown file was not created.");

        var fileContent = File.ReadAllText(outputFileName);
        var expectedContent = "# TestFile\n\nThis is a test content.";
        Assert.Equal(expectedContent, fileContent);

        File.Delete(outputFileName);
    }
    
    [Fact]
    public void Export_CreatesPdfFileWithCorrectContent()
    {
        var fileDto = file();
        var outputFileName = "TestFile.pdf";
        
        _exporterService.ExportToPdf(fileDto);
        
        Assert.True(File.Exists(outputFileName), "PDF file was not created.");

        var fileContent = File.ReadAllText(outputFileName);
        var expectedContent = "TestFile\n\nThis is a test content.";
        Assert.Equal(expectedContent, fileContent);

        File.Delete(outputFileName);
    }
    
    [Fact]
    public void Export_CreatesWordFileWithCorrectContent()
    {
        var fileDto = file();
        var outputFileName = "TestFile.docx";
        
        _exporterService.ExportToPdf(fileDto);
        
        Assert.True(File.Exists(outputFileName), "Word file was not created.");

        var fileContent = File.ReadAllText(outputFileName);
        var expectedContent = "TestFile\n\nThis is a test content.";
        Assert.Equal(expectedContent, fileContent);

        File.Delete(outputFileName);
    }
}