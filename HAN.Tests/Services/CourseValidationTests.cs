using HAN.Services;
using HAN.Services.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class CourseValidationTests : TestBase
{
    private readonly ICourseValidationService _courseValidationService;
    
    public CourseValidationTests()
    {
        _courseValidationService = ServiceProvider.GetRequiredService<ICourseValidationService>();
    }

    // [Fact]
    // public void ValidateCourse_ShouldReturnTrue()
    // {
    //     var validCourseId = TestDbSeederValidateCourse.SeedValidCourseForValidation(Context);
    //     var valid = _courseValidationService.ValidateCourse(validCourseId);
    //     
    //     Assert.True(valid);
    // }
    //
    // [Fact]
    // public void ValidateCourse_ShouldReturnFalse()
    // {
    //     var invalidCourseId = TestDbSeederValidateCourse.SeedInvalidLessonCourseForValidation(Context);
    //     var valid = _courseValidationService.ValidateCourse(invalidCourseId);
    //     
    //     Assert.False(valid);
    // }
    //
    // [Fact]
    // public void ValidateCourse_ShouldReturnFalse_WhenCourseDoesNotExist()
    // {
    //     const int nonExistentCourse = 1000;
    //     var valid = false;
    //     
    //     var expectedException = Record.Exception(() =>
    //     {
    //         valid = _courseValidationService.ValidateCourse(nonExistentCourse);
    //     });
    //
    //     Assert.NotNull(expectedException);
    //     Assert.IsType<KeyNotFoundException>(expectedException);
    //     Assert.False(valid);
    // }
    //
    // [Fact]
    // public void ValidateCourse_ShouldReturnFalse_WhenCourseHasNoEvls()
    // {
    //     TestDbSeeder.SeedCourses(Context, 1);
    //     const int courseId = 1;
    //     
    //     var valid = _courseValidationService.ValidateCourse(courseId);
    //     
    //     Assert.False(valid);
    // }
}