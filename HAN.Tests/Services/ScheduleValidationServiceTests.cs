using HAN.Services;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class ScheduleValidationServiceTests : TestBase
{
    private readonly ICoursePlanningValidationService _coursePlanningValidationService;

    public ScheduleValidationServiceTests()
    {
        _coursePlanningValidationService = ServiceProvider.GetRequiredService<ICoursePlanningValidationService>();
    }
    
    [Fact]
    public void ValidateCourse_ShouldReturnTrue()
    {
        var courseWithValidCoursePlanningId = TestDbSeederValidateCourse.SeedValidCourseForValidation(Context);

        var valid = _coursePlanningValidationService.ValidateCoursePlanning(courseWithValidCoursePlanningId);
        
        Assert.True(valid);
    }
    
    [Fact]
    public void ValidateCourse_ShouldReturnFalse()
    {
        var courseWithInvalidCoursePlanningId = TestDbSeederValidateCourse.SeedInvalidLessonCourseForValidation(Context);

        var valid = _coursePlanningValidationService.ValidateCoursePlanning(courseWithInvalidCoursePlanningId);
        
        Assert.False(valid);
    }
}