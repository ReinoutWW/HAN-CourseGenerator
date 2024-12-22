﻿using HAN.Services;
using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class CourseServiceTests : TestBase
{
    private readonly ICourseService _courseService;
    private readonly IEvlService _evlService;
    private readonly LessonService _lessonService;
    private readonly ExamService _examService;
    private readonly CourseComponentService _courseComponentService;
    private readonly ICourseValidationService _courseValidationService;

    public CourseServiceTests()
    {
        _courseService = ServiceProvider.GetRequiredService<ICourseService>();
        _evlService = ServiceProvider.GetRequiredService<IEvlService>();
        _lessonService = ServiceProvider.GetRequiredService<LessonService>();
        _examService = ServiceProvider.GetRequiredService<ExamService>();
        _courseComponentService = ServiceProvider.GetRequiredService<CourseComponentService>();
        _courseValidationService = ServiceProvider.GetRequiredService<ICourseValidationService>();
    }

    [Fact]
    public void CreateCourse_ShouldCreateCourse()
    {
        CourseDto course = new()
        {
            Name = "Test Course",
            Description = "Test Description",
        };

        var createdCourse = _courseService.CreateCourse(course);
        
        Assert.NotNull(createdCourse);
        Assert.Equal(course.Name, createdCourse.Name);
        Assert.Equal(course.Description, createdCourse.Description);
    }

    [Fact]
    public void CreateCourse_ShouldThrowException_WhenNameIsNull()
    {   
        Assert.Throws<ArgumentNullException>(() => _courseService.CreateCourse(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("..")]
    [InlineData("industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of sheets containing Lorem Ipsum passages, and more rec")]
    [InlineData(null)]
    public void CreateCourse_ShouldThrowException_WhenNameIsInvalid(string invalidName)
    {
        CourseDto course = new() { Name = invalidName  };
        CreateCourseExpectValidationException(course);
    }

    [Theory]
    [InlineData("\"Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?\"\n\n1914 translation by H. Rackham\n\"But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?\"")]
    public void CreateCourse_ShouldThrowException_WhenDescriptionIsInvalid(string invalidDescription)
    {
        CourseDto course = new()
        {
            Name = "Valid name",
            Description = invalidDescription
        };
        
        CreateCourseExpectValidationException(course);
    }

    [Fact]
    public void AddEvlToCourse_ShouldAddEvl()
    {
        var courseId = SeedValidCourse();
        var evl = SeedEvls().FirstOrDefault();

        var course = _courseService.GetCourseById(courseId);

        var exception = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(course.Id, evl.Id);
        });
        
        Assert.Null(exception);
    }

    [Fact]
    public void AddEvlToCourse_ShouldThrowException_EvlNotFound()
    {
        var courseId = SeedValidCourse();
        
        var course = _courseService.GetCourseById(courseId);
        const int nonExistentEvilId = 100000;
        
        var expectedException = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(course.Id, nonExistentEvilId);
        });
        
        Assert.NotNull(expectedException);
        Assert.IsType<KeyNotFoundException>(expectedException);
    }
    
    [Fact]
    public void AddEvlToCourse_ShouldThrowException_CourseNotFound()
    {
        var evl = SeedEvls().FirstOrDefault();
        const int nonExistentCourseId = 100000;

        var expectedException = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(nonExistentCourseId, evl.Id);
        });
        
        Assert.NotNull(expectedException);
        Assert.IsType<KeyNotFoundException>(expectedException);
    }

    [Fact]
    public void AddEvlToCourse_ShouldThrowException_EvlAlreadyAdded()
    {
        var courseId = SeedValidCourse();
        var evlId = SeedEvls().FirstOrDefault().Id;
        
        var evl = _evlService.GetEvlById(evlId);
        var course = _courseService.GetCourseById(courseId);
        
        var expectedExceptionNull = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(course.Id, evl.Id);
        });
        
        var expectedException = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(course.Id, evl.Id);
        });
        
        Assert.Null(expectedExceptionNull);
        Assert.NotNull(expectedException);
        Assert.IsType<InvalidOperationException>(expectedException);
    }

    [Fact]
    private void GetAllEvls_ShouldReturnAllEvls()
    {
        var evlCOunt = 2;
        var courseId = SeedValidCourse(evlCOunt);
        var evlIds = SeedEvls(1);
        var evlId = evlIds.FirstOrDefault().Id;
        
        var course = _courseService.GetCourseById(courseId);
        
        var expectedExceptionNull = Record.Exception(() =>
        {
            _courseService.AddEvlToCourse(course.Id, evlId);
            evlCOunt++;
        });

        var evls = _courseService.GetEvls(course.Id).ToList();
        
        Assert.Null(expectedExceptionNull);
        Assert.NotNull(evls);
        Assert.Equal(expected: 3, evls.Count);
    }
    
    private void CreateCourseExpectValidationException(CourseDto course)
    {
        var expectedException = Record.Exception(() =>
        {
            _courseService.CreateCourse(course);
        });

        Assert.NotNull(expectedException);
        Assert.IsType<HAN.Services.Exceptions.ValidationException>(expectedException);
    }
    
    [Fact]
    public void Test()
    {
        var evls = SeedEvls();
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithEvls(evls)
            .Build();
        
        var createdCourse = _courseService.CreateCourse(course);
        
        var createdEvls = _courseService.GetEvls(createdCourse.Id).ToList();
        
        Assert.NotNull(createdCourse);
        Assert.NotNull(createdEvls);
        Assert.Equal(course.Name, createdCourse.Name);
        Assert.Equal(course.Description, createdCourse.Description);
        Assert.Equal(course.Evls.Count, createdEvls.Count);
    }
    
    [Fact]
    public void AddSchedule_ShouldAddSchedule()
    {
        var courseId = SeedValidCourse();

        var scheduleDto = new ScheduleDto();

        _courseService.AddSchedule(scheduleDto, courseId);
        
        var course = _courseService.GetCourseById(courseId);
        
        Assert.NotNull(course.Schedule);
        Assert.Equal(scheduleDto.ScheduleLines.Count, course.Schedule.ScheduleLines.Count);
    }
    
    [Fact]
    public void GetSchedule_ShouldReturnSchedule()
    {
        var courseId = SeedValidCourse();
        var scheduleDto = new ScheduleDto();

        _courseService.AddSchedule(scheduleDto, courseId);
        
        var course = _courseService.GetCourseById(courseId);
        
        var schedule = _courseService.GetScheduleById(course.Schedule.Id);
                
        Assert.NotNull(schedule);
        Assert.Equal(scheduleDto.ScheduleLines.Count, schedule.ScheduleLines.Count);
    }

    [Fact]
    public void ValidateSchedule_ShouldBeComplete()
    {
        var courseId = SeedCourseWithCompleteSchedule();
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
        var evls = SeedEvls();
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithEvls(evls)
            .Build();
        
        course = _courseService.CreateCourse(course);
        
        AddLessonToEvls(course.Evls);
        AddExamToEvls(course.Evls);

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
        var evls = SeedEvls();
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithEvls(evls)
            .Build();
        
        course = _courseService.CreateCourse(course);
        
        AddLessonToEvls(course.Evls);
        AddExamToEvls(course.Evls);

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
        var evls = SeedEvls();
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithEvls(evls)
            .Build();
        
        course = _courseService.CreateCourse(course);
        
        AddLessonToEvls(course.Evls);
        
        _courseService.AddSchedule(new ScheduleDto(), course.Id);
        
        var complete = _courseValidationService.IsCourseComplete(course.Id);
        
        Assert.False(complete);
    }

    [Fact]
    public void Schedule_Update_ShouldUpdate()
    {
        var courseId = SeedCourseWithCompleteSchedule();
        var evl = SeedEvls(1).FirstOrDefault();
        var course = _courseService.GetCourseById(courseId);
        
        _courseService.AddEvlToCourse(courseId, evl.Id);
        AddLessonToEvls([evl]);
        
        var evlIds = course.Evls.Select(x => x.Id).ToList();
        var courseComponents = _courseComponentService.GetAllCourseComponentByEvlIds(evlIds);
        
        course = new CourseDtoBuilder(course)
            .WithInvalidSchedule(courseComponents)
            .Build();
        
        var schedule = _courseService.AddSchedule(course.Schedule, course.Id);
        
        Assert.NotNull(schedule);
        Assert.Equal(course.Schedule.ScheduleLines.Count, schedule.ScheduleLines.Count);
    }

    private static void ScheduleShouldContainAllCourseComponents(ScheduleDto schedule, List<CourseComponentDto> courseComponents)
    {
        foreach (var courseComponent in courseComponents)
        {
            var scheduleLine = schedule.ScheduleLines.FirstOrDefault(x => x.CourseComponentId == courseComponent.Id);
            Assert.NotNull(scheduleLine);
        }
    }
    
    private int SeedCourseWithCompleteSchedule()
    {
        var evls = SeedEvls();
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithEvls(evls)
            .Build();
        
        course = _courseService.CreateCourse(course);
        
        AddLessonToEvls(course.Evls);

        var evlIds = course.Evls.Select(x => x.Id).ToList();
        var courseComponents = _courseComponentService.GetAllCourseComponentByEvlIds(evlIds);

        course = new CourseDtoBuilder(course)
            .WithValidSchedule(courseComponents)
            .Build();

        _courseService.AddSchedule(course.Schedule, course.Id);
        
        return course.Id;
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
    
    private void AddExamToEvls(List<EvlDto> evls)
    {
        for(var i = 0; i < evls.Count; i++)
        {
            var evl = evls[i];
            var exam = (ExamDto)new CourseComponentDtoBuilder()
                .AsExam()
                .WithName("Lesson 1")
                .Build();

            exam.Evls ??= [];
            exam.Evls.Add(evl);
            
            _examService.CreateCourseComponent(exam);
        }
    }
    
    private int SeedValidCourse(int evlCount = 2)
    {
        var evls = SeedEvls(evlCount);
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithEvls(evls)
            .Build();

        var createdCourse = _courseService.CreateCourse(course);
        
        return createdCourse.Id;
    }
    
    private List<EvlDto> SeedEvls(int evlCount = 2)
    {
        List<EvlDto> evls = [];
        
        for(var i = 0; i < evlCount; i++)
        {
            var evl = new EvlDto
            {
                Name = "Test Evl",
                Description = "Test Description",
            };
        
            var createdEvl = _evlService.CreateEvl(evl);
            evls.Add(createdEvl);
        }

        return evls;
    }
    
}