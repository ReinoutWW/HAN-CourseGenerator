using HAN.Services;
using HAN.Tests.Base;

namespace HAN.Tests.Services;

public class CourseValidationTests : TestBase
{
    private readonly ICourseService _courseService;
    
    public CourseValidationTests(ICourseService courseService)
    {
        _courseService = courseService;
        //TestDbSeeder.SeedCoursesForValidation(Context, 2);
    }

    [Fact]
    public void ValidateCourse_ShouldReturnTrue()
    {
        // Arrange
        
        
        // Act

        
        // Assert
    }
}