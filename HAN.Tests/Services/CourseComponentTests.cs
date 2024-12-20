using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class CourseComponentTests : TestBase
{
    private readonly ICourseComponentService _courseComponentService;
    private readonly IFileService _fileService;

    public CourseComponentTests()
    {
        _courseComponentService = ServiceProvider.GetRequiredService<ICourseComponentService>();
        _fileService = ServiceProvider.GetRequiredService<IFileService>();
    }

    [Fact]
    public void CreateLesson_ShouldCreateLesson()
    {
        // Arrange
        LessonDto lesson = new()
        {
            Name = "Lesson",
            Difficulty = 1
        };

        // Act
        var createdLesson = _courseComponentService.CreateCourseComponent(lesson);

        // Assert
        Assert.NotNull(createdLesson);
        Assert.IsType<LessonDto>(createdLesson);
        Assert.Equal(lesson.Name, createdLesson.Name);
        Assert.Equal(lesson.Difficulty, createdLesson.Difficulty);
    }

    [Fact]
    public void AddFile_ShouldAddFileToLesson()
    {
        // Arrange
        const int seedLessonId = 1;
        TestDbSeeder.SeedLessons(Context, 1);

        var existingLesson = _courseComponentService.GetCourseComponentById<LessonDto>(seedLessonId);
        FileDto file = new()
        {
            Name = "FileName",
            Content = "FileContent"
        };

        var createdFile = _fileService.CreateFile(file);

        // Act
        _courseComponentService.AddFileToCourseComponent(existingLesson.Id, createdFile.Id);
        var filesForLesson = _courseComponentService.GetFilesForCourseComponent(existingLesson.Id);

        // Assert
        var containsItem = filesForLesson.Any(item => createdFile.Id == item.Id);
        Assert.True(containsItem);
    }

    [Fact]
    public void AddEvl_ShouldAddEvlToLesson()
    {
        // Arrange
        const int seedLessonId = 1;
        TestDbSeeder.SeedLessons(Context, 1);

        var existingLesson = _courseComponentService.GetCourseComponentById<LessonDto>(seedLessonId);
        EvlDto evl = new()
        {
            Name = "EvlName",
            Description = "EvlDescription"
        };

        var createdEvl = ServiceProvider.GetRequiredService<IEvlService>().CreateEvl(evl);

        // Act
        _courseComponentService.AddEvlToCourseComponent(existingLesson.Id, createdEvl.Id);
        var evlsForLesson = _courseComponentService.GetEvlsForCourseComponent(existingLesson.Id);

        // Assert
        var containsItem = evlsForLesson.Any(item => createdEvl.Id == item.Id);
        Assert.True(containsItem);
    }

    [Fact]
    public void Lesson_HasEvls_ShouldHaveEvls()
    {
        // Arrange
        const int seedLessonId = 1;
        const int evlCount = 1;
        TestDbSeeder.SeedLessonsWithEvls(Context, 1, evlCount);

        var existingLesson = _courseComponentService.GetCourseComponentById<LessonDto>(seedLessonId);
        
        // Act
        var evlsForLesson = _courseComponentService.GetEvlsForCourseComponent(existingLesson.Id);
        
        // Assert
        Assert.NotEmpty(evlsForLesson);
        Assert.Single(evlsForLesson);
    }
}
