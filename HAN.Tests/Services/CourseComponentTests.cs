using HAN.Services;
using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class CourseComponentTests : TestBase
{
    private readonly LessonService _lessonService;
    private readonly ExamService _examService;
    private readonly IFileService _fileService;
    private readonly IEvlService _evlService;

    public CourseComponentTests()
    {
        _lessonService = ServiceProvider.GetRequiredService<LessonService>();
        _examService = ServiceProvider.GetRequiredService<ExamService>();
        _fileService = ServiceProvider.GetRequiredService<IFileService>();
        _evlService = ServiceProvider.GetRequiredService<IEvlService>();
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
        var createdLesson = _lessonService.CreateCourseComponent(lesson);

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
        var evl = SeedEvl();
        var seedLessonId = SeedLessons(evl);

        var existingLesson = _lessonService.GetCourseComponentById(seedLessonId);
        FileDto file = new()
        {
            Name = "FileName",
            Content = "FileContent"
        };

        var createdFile = _fileService.CreateFile(file);

        // Act
        _lessonService.AddFileToCourseComponent(existingLesson.Id, createdFile.Id);
        var filesForLesson = _lessonService.GetFilesForCourseComponent(existingLesson.Id);

        // Assert
        var containsItem = filesForLesson.Any(item => createdFile.Id == item.Id);
        Assert.True(containsItem);
    }

    [Fact]
    public void AddEvl_ShouldAddEvlToLesson()
    {
        // Arrange
        var evl = SeedEvl();
        var seedLessonId = SeedLessons(evl);
        
        var existingLesson = _lessonService.GetCourseComponentById(seedLessonId);

        var secondEvl = SeedEvl();
        // Act
        _lessonService.AddEvlToCourseComponent(existingLesson.Id, secondEvl.Id);
        var evlsForLesson = _lessonService.GetEvlsForCourseComponent(existingLesson.Id);

        // Assert
        var containsItem = evlsForLesson.Any(item => secondEvl.Id == item.Id);
        Assert.True(containsItem);
    }

    [Fact]
    public void Lesson_HasEvls_ShouldHaveEvls()
    {
        // Arrange
        var evl = SeedEvl();
        var seedLessonId = SeedLessons(evl);

        var existingLesson = _lessonService.GetCourseComponentById(seedLessonId);
        
        // Act
        var evlsForLesson = _lessonService.GetEvlsForCourseComponent(existingLesson.Id);
        
        // Assert
        Assert.NotEmpty(evlsForLesson);
        Assert.Single(evlsForLesson);
    }

    [Fact]
    public void CreateExam_ShouldCreateExam()
    {
        ExamDto exam = new()
        {
            Name = "Valid exam name",
            Score = 10
        };
        
        var createdExam = _examService.CreateCourseComponent(exam);
        
        Assert.NotNull(createdExam);
        Assert.Equal(exam.Name, createdExam.Name);
        Assert.Equal(exam.Score, createdExam.Score);
    }

    private int SeedLessons(EvlDto evls)
    {
        var lesson = new CourseComponentDtoBuilder()
            .AsLesson()
            .WithEvl(evls)
            .Build();
        
        var createdLesson = _lessonService.CreateCourseComponent((LessonDto)lesson);

        return createdLesson.Id;
    }
    
    private EvlDto SeedEvl()
    {
        var evl = new EvlDto()
        {
            Name = "Example evl",
            Description = "Example evl description"
        };
        
        var createdEvl = _evlService.CreateEvl(evl);
        return createdEvl;
    } 
}
