using HAN.Data;
using HAN.Services;
using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class CourseScheduleTests : TestBase
{
    private readonly IScheduleService _scheduleService;
    private readonly ICourseService _courseService;
    private readonly LessonService _lessonService;
    
    public CourseScheduleTests()
    {
        _scheduleService = ServiceProvider.GetRequiredService<IScheduleService>();
        _courseService = ServiceProvider.GetRequiredService<ICourseService>();
        _lessonService = ServiceProvider.GetRequiredService<LessonService>();
    }
    
    [Fact]
    public void AddSchedule_ShouldAddSchedule()
    {
        var courseId = SeedValidCourse();

        var scheduleDto = new ScheduleDto();

        _scheduleService.CreateSchedule(scheduleDto, courseId);
        
        var course = _courseService.GetCourseById(courseId);
        
        Assert.NotNull(course.Schedule);
        Assert.Equal(scheduleDto.ScheduleLines.Count, course.Schedule.ScheduleLines.Count);
    }
    
    [Fact]
    public void GetSchedule_ShouldReturnSchedule()
    {
        var courseId = SeedValidCourse();
        var scheduleDto = new ScheduleDto();

        _scheduleService.CreateSchedule(scheduleDto, courseId);
        
        var course = _courseService.GetCourseById(courseId);
        
        var schedule = _scheduleService.GetScheduleById(course.Schedule.Id);
        
        Assert.NotNull(schedule);
        Assert.Equal(scheduleDto.ScheduleLines.Count, schedule.ScheduleLines.Count);
    }
    
    private int SeedValidCourse(int seedEvlCount = 1)
    {
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithCreatedEvls(seedEvlCount)
            .Build();
        
        AddLessonToEvls(course.Evls);

        var createdCourse = _courseService.CreateCourse(course);
        
        return createdCourse.Id;
    }

    private void AddLessonToEvls(List<EvlDto> evls)
    {
        for(var i = 0; i < evls.Count; i++)
        {
            var evl = evls[i];
            var lesson = (LessonDto)new CourseComponentDtoBuilder()
                .AsLesson()
                .WithName("Lesson 1")
                .Build();

            lesson.Evls ??= [];
            lesson.Evls.Add(evl);
            
            _lessonService.CreateCourseComponent(lesson);
        }
        
    }
}