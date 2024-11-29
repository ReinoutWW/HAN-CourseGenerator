using HAN.Services;
using HAN.Services.DTOs;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests;

public class CourseServiceTests : TestBase
{
    private readonly ICourseService _courseService;
    
    public CourseServiceTests()
    {
        _courseService = ServiceProvider.GetRequiredService<ICourseService>();
    }

    [Fact]
    public void CreateCourse_ShouldCreateCourse()
    {
        CreateCourseDto course = new()
        {
            Name = "Test Course",
            Description = "Test Description",
        };

        var createdCourse = _courseService.CreateCourse(course);
        
        Assert.NotNull(createdCourse);
        Assert.Equal(course.Name, createdCourse.Name);
        Assert.Equal(course.Description, createdCourse.Description);
    }
}