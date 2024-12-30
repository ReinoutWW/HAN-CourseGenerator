using HAN.Services;
using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using HAN.Tests.Builders;

namespace HAN.Tests.Base;

public class PersistHelper
{
    private readonly ICourseService _courseService;
    private readonly IEvlService _evlService;
    private readonly LessonService _lessonService;
    private readonly ExamService _examService;
    private readonly CourseComponentService _courseComponentService;

    public PersistHelper(
        ICourseService courseService,
        IEvlService evlService,
        LessonService lessonService,
        CourseComponentService courseComponentService,
        ExamService examService
        )
    {
        _courseService = courseService;
        _evlService = evlService;
        _lessonService = lessonService;
        _courseComponentService = courseComponentService;
        _examService = examService;
    }
    
    public int SeedLessons(EvlDto evls)
    {
        var lesson = new CourseComponentDtoBuilder()
            .AsLesson()
            .WithEvl(evls)
            .Build();
        
        var createdLesson = _lessonService.CreateCourseComponent((LessonDto)lesson);

        return createdLesson.Id;
    }
    
    public int SeedCourseWithCompleteSchedule()
    {
        var evls = SeedEvls();
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithEvls(evls)
            .Build();
        
        course = _courseService.CreateCourse(course);
        
        SeedLessonToEvls(course.Evls);

        var evlIds = course.Evls.Select(x => x.Id).ToList();
        var courseComponents = _courseComponentService.GetAllCourseComponentByEvlIds(evlIds);

        course = new CourseDtoBuilder(course)
            .WithValidSchedule(courseComponents)
            .Build();

        _courseService.UpdateCourse(course);
        
        return course.Id;
    }
    
    public void SeedLessonToEvls(List<EvlDto> evls)
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
    
    public void SeedExamToEvls(List<EvlDto> evls)
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
    
    public int SeedValidCourse(int evlCount = 2)
    {
        var evls = SeedEvls(evlCount);
        var course = new CourseDtoBuilder()
            .WithName("Course name")
            .WithEvls(evls)
            .Build();

        var createdCourse = _courseService.CreateCourse(course);
        
        return createdCourse.Id;
    }
    
    public EvlDto SeedEvl()
    {
        var evl = new EvlDto()
        {
            Name = "Example evl",
            Description = "Example evl description"
        };
        
        var createdEvl = _evlService.CreateEvl(evl);
        return createdEvl;
    } 
    
    public List<EvlDto> SeedEvls(int evlCount = 2)
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