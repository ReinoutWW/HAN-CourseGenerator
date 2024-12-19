using HAN.Services;
using HAN.Services.DTOs;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class FileServiceTests: TestBase
{
    private readonly IFileService _fileService;
    
    public FileServiceTests()
    {
        _fileService = ServiceProvider.GetRequiredService<IFileService>();
    }

    [Fact]
    public void CreateFile_ShouldCreateFile()
    {
        FileDto file = new()
        {
            Name = "Filename",
            Content = "Content"
        };

        var createdfile = _fileService.CreateFile(file);
        
        Assert.NotNull(createdfile);
        Assert.Equal(file.Name, createdfile.Name);
        Assert.Equal(file.Content, createdfile.Content);
    }

    [Fact]
    public void CreateFile_ShouldThrowException_WhenNameIsNull()
    {  
        Assert.Throws<ArgumentNullException>(() => _fileService.CreateFile(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(".")]
    [InlineData("industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of sheets containing Lorem Ipsum passages, and more rec")]
    [InlineData(null)]
    public void CreateFile_ShouldThrowException_WhenNameIsInvalid(string invalidName)
    {
        FileDto file = new()
        {
            Name = invalidName,
            Content = "Content"
        };
        
        CreateFileExpectValidationException(file);
    }

    private void CreateFileExpectValidationException(FileDto file)
    {
        var expectedException = Record.Exception(() =>
        {
            _fileService.CreateFile(file);
        });

        Assert.NotNull(expectedException);
        Assert.IsType<HAN.Services.Exceptions.ValidationException>(expectedException);
    }
}