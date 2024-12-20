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
    private readonly ICourseValidationService _courseValidationService;
    private readonly CourseComponentService _courseComponentService;

    public CourseScheduleTests()
    {
        _scheduleService = ServiceProvider.GetRequiredService<IScheduleService>();
        _courseService = ServiceProvider.GetRequiredService<ICourseService>();
        _lessonService = ServiceProvider.GetRequiredService<LessonService>();
        _courseComponentService = ServiceProvider.GetRequiredService<CourseComponentService>();
        _courseValidationService = ServiceProvider.GetRequiredService<ICourseValidationService>();
    }
    
    [Fact]
    public void AddSchedule_ShouldAddSchedule()
    {
        var courseId = SeedCourseWithCompleteSchedule();

        var scheduleDto = new ScheduleDto();

        _scheduleService.AddSchedule(scheduleDto, courseId);
        
        var course = _courseService.GetCourseById(courseId);
        
        Assert.NotNull(course.Schedule);
        Assert.Equal(scheduleDto.ScheduleLines.Count, course.Schedule.ScheduleLines.Count);
    }
    
    [Fact]
    public void GetSchedule_ShouldReturnSchedule()
    {
        var courseId = SeedCourseWithCompleteSchedule();
        var scheduleDto = new ScheduleDto();

        _scheduleService.AddSchedule(scheduleDto, courseId);
        
        var course = _courseService.GetCourseById(courseId);
        
        var schedule = _scheduleService.GetScheduleById(course.Schedule.Id);
                
        Assert.NotNull(schedule);
        Assert.Equal(scheduleDto.ScheduleLines.Count, schedule.ScheduleLines.Count);
    }

    [Fact]
    public void GetSchedule_ShouldBeComplete()
    {
        var courseId = SeedCourseWithCompleteSchedule();
        var course = _courseService.GetCourseById(courseId);
        
        var schedule = _scheduleService.GetScheduleById(course.Schedule.Id);
        
        var courseComponents = _courseComponentService.GetAllCourseComponentsByCourseId(courseId);

        var complete = _courseValidationService.IsCourseComplete(courseId);
        
        ScheduleShouldContainAllCourseComponents(schedule, courseComponents);
        Assert.NotNull(schedule);
        Assert.True(complete);
    }

    [Fact]
    public void GetSchedule_ShouldBeIncomplete()
    {
        var courseId = SeedCourseWithIncompleteSchedule();
        var course = _courseService.GetCourseById(courseId);
        
        var schedule = _scheduleService.GetScheduleById(course.Schedule.Id);
        
        var complete = _courseValidationService.IsCourseComplete(courseId);
        
        Assert.NotNull(schedule);
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
    
    private int SeedCourseWithIncompleteSchedule(int seedEvlCount = 1)
    {
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithCreatedEvls(seedEvlCount)
            .Build();
        
        AddLessonToEvls(course.Evls);

        var createdCourse = _courseService.CreateCourse(course);
        
        _scheduleService.AddSchedule(new ScheduleDto(), createdCourse.Id);

        return createdCourse.Id;
    }
    
    private int SeedCourseWithCompleteSchedule(int seedEvlCount = 1)
    {
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithCreatedEvls(seedEvlCount)
            .Build();
        
        AddLessonToEvls(course.Evls);

        var createdCourse = _courseService.CreateCourse(course);
        
        CreateScheduleWithAllLessons(createdCourse);
        return createdCourse.Id;
    }

    private void CreateScheduleWithAllLessons(CourseDto createdCourse)
    {
        var courseComponents = _lessonService.GetAllCourseComponentsByCourseId(createdCourse.Id);

        var sequenceId = 1;
        var scheduleDto = new ScheduleDto();
        foreach (var courseComponent in courseComponents)
        {
            var scheduleLine = new ScheduleLineDto()
            {
                WeekSequenceNumber = sequenceId,
                CourseComponent = courseComponent,
                CourseComponentId = courseComponent.Id
            };
            
            scheduleDto.ScheduleLines.Add(scheduleLine);
            sequenceId++;
        }

        _scheduleService.AddSchedule(scheduleDto, createdCourse.Id);
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