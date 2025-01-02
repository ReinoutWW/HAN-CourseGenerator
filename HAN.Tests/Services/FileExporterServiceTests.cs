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
}