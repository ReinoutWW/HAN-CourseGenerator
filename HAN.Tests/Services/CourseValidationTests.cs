using HAN.Services;
using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class CourseValidationTests : TestBase
{
    private readonly ICourseValidationService _courseValidationService;
    private readonly ICourseService _courseService;
    private readonly PersistHelper _persistHelper;
    private readonly CourseComponentService _courseComponentService;

    public CourseValidationTests()
    {
        _courseValidationService = ServiceProvider.GetRequiredService<ICourseValidationService>();
        _courseComponentService = ServiceProvider.GetRequiredService<CourseComponentService>();
        _courseService = ServiceProvider.GetRequiredService<ICourseService>();
        _persistHelper = ServiceProvider.GetRequiredService<PersistHelper>();
    }

    [Fact]
    public void ValidateSchedule_ShouldBeComplete()
    {
        var courseId = _persistHelper.SeedCourseWithCompleteSchedule();
        var course = _courseService.GetCourseById(courseId);
        var evlIds = course.Evls.Select(evl => evl.Id).ToList();
        
        var schedule = _courseService.GetScheduleById(course.Schedule.Id);
        
        var courseComponents = _courseComponentService.GetAllCourseComponentByEvlIds(evlIds);

        var complete = _courseValidationService.IsCourseComplete(courseId);
        
        ScheduleShouldContainAllCourseComponents(schedule, courseComponents);
        Assert.NotNull(schedule);
        Assert.True(complete);
    }
    
    [Fact]
    public void ValidateSchedule_ShouldBeValid()
    {
        var evls = _persistHelper.SeedEvls();
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithEvls(evls)
            .Build();
        
        course = _courseService.CreateCourse(course);
        
        _persistHelper.SeedLessonToEvls(course.Evls);
        _persistHelper.SeedExamToEvls(course.Evls);

        var evlIds = course.Evls.Select(x => x.Id).ToList();
        var courseComponents = _courseComponentService.GetAllCourseComponentByEvlIds(evlIds);

        course = new CourseDtoBuilder(course)
            .WithValidSchedule(courseComponents)
            .Build();

        _courseService.AddSchedule(course.Schedule, course.Id);
        
        var valid = _courseValidationService.ValidateCourse(course.Id);
        
        Assert.True(valid);
    }

    [Fact]
    public void ValidateSchedule_ShouldBeInvalid()
    {
        var evls = _persistHelper.SeedEvls();
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithEvls(evls)
            .Build();
        
        course = _courseService.CreateCourse(course);
        
        _persistHelper.SeedLessonToEvls(course.Evls);
        _persistHelper.SeedExamToEvls(course.Evls);

        var evlIds = course.Evls.Select(x => x.Id).ToList();
        var courseComponents = _courseComponentService.GetAllCourseComponentByEvlIds(evlIds);

        course = new CourseDtoBuilder(course)
            .WithInvalidSchedule(courseComponents)
            .Build();

        _courseService.AddSchedule(course.Schedule, course.Id);
        
        var valid = _courseValidationService.ValidateCourse(course.Id);
        
        Assert.True(valid);
    }

    [Fact]
    public void ValidateSchedule_ShouldBeIncomplete()
    {
        var evls = _persistHelper.SeedEvls();
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithEvls(evls)
            .Build();
        
        course = _courseService.CreateCourse(course);
        
        _persistHelper.SeedLessonToEvls(course.Evls);
        
        _courseService.AddSchedule(new ScheduleDto(), course.Id);
        
        var complete = _courseValidationService.IsCourseComplete(course.Id);
        
        Assert.False(complete);
    }
    
    private static void ScheduleShouldContainAllCourseComponents(ScheduleDto schedule, List<CourseComponentDto> courseComponents)
    {
        foreach (var courseComponent in courseComponents)
        {
            var scheduleLine = schedule.ScheduleLines.FirstOrDefault(x => x.CourseComponentId == courseComponent.Id);
            Assert.NotNull(scheduleLine);
        }
    }
}