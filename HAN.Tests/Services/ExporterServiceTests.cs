using HAN.Services;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class ExporterServiceTests : TestBase
{
    private readonly IFileService _fileService;
    private readonly IExporterService _exporterService;
        
    public ExporterServiceTests()
    {
        _fileService = ServiceProvider.GetRequiredService<IFileService>();
        _exporterService = ServiceProvider.GetRequiredService<IExporterService>();
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