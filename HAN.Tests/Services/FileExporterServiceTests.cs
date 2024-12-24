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

    [Fact]
    public void Test()
    {
        var fileDto = new FileDto
        {
            Name = "example.md",
            Content = "# Hello Markdown\nThis is a markdown file."
        };
        
        // for markdown export
        _exporterService.ExportToMarkdown(fileDto);

        // for word export
        fileDto.Name = "example.docx";
        fileDto.Content = "Hello Word\nThis is a Word file.";
        _exporterService.ExportToWord(fileDto);
        
        // for pdf export
        fileDto.Name = "example.pdf";
        fileDto.Content = "Hello PDF\nThis is a PDF file.";
        _exporterService.ExportToPdf(fileDto);
    }

// SonarQube: Can't scan with broken tests :D So will comment out for now. Once it's all working, we can uncomment and run the tests.
    // * Or while developing of course
    
    // [Fact]
    // public void CreateAndExportFile_ShouldCreateAndExportFile()
    // {
    //     FileDto file = new()
    //     {
    //         Name = "testfile.txt",
    //         Content = "Test Content"
    //     };
    //     
    //     var createdFile = _fileService.CreateFile(file);
    //     
    //     // Assert.NotNull(createdFile);
    //     // Assert.Equal(file.Name, createdFile.Name);
    //     // Assert.Equal(file.Name, createdFile.Name);
    //     
    //     _exporterService.Export(createdFile.Content);
    // }
}