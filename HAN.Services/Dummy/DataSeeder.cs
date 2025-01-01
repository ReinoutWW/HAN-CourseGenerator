using HAN.Services;
using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Services.Dummy;

public static class DataSeeder
{
    public static void SeedCourseData(IServiceProvider serviceProvider)
    {
        int iterations = 15;
        
        for (int i = 0; i < iterations; i++)
        {
            SeedCourseIteration(i, serviceProvider);
        }
    }

    private static void SeedCourseIteration(int iteration, IServiceProvider serviceProvider)
    {
        var courseService = serviceProvider.GetRequiredService<ICourseService>();

        var firstRandomEvl = SeedRandomEvl(serviceProvider);
        var secondRandomEvl = SeedRandomEvl(serviceProvider);
        
        var courseName = GetRandomCourseName();
        var course = new CourseDto()
        {
            Name = courseName,
            Description = $"({iteration}): Introduction to {courseName}. After this course, you will be able to understand the basics of {courseName}.",
            Evls = [firstRandomEvl, secondRandomEvl]
        };

        courseService.CreateCourse(course);
    }

    private static EvlDto SeedRandomEvl(IServiceProvider serviceProvider)
    {
        var evlService = serviceProvider.GetRequiredService<IEvlService>();
        var lessonService = serviceProvider.GetRequiredService<LessonService>();
        var examService = serviceProvider.GetRequiredService<ExamService>();

        var randomEvlName = GetRandomEvlName();
        
        var randomEvl = new EvlDto()
        {
            Name = $"{randomEvlName}",
            Description = $"Learning everything about {randomEvlName}",
        };
        
        randomEvl = evlService.CreateEvl(randomEvl);

        var lessons = new List<LessonDto>()
        {
            new LessonDto()
            {
                Name = $"Learning the basics",
                Description = $"Introduction lesson to {randomEvlName}",
                Evls = [randomEvl]
            },
            new LessonDto()
            {
                Name = $"Learning the advanced",
                Description = $"Introduction to advanced {randomEvlName}",
                Evls = [randomEvl]
            }
        };

        lessons.ForEach(lesson => lessonService.CreateCourseComponent(lesson));

        var exam = new ExamDto()
        {
            Name = $"Final exam {randomEvlName}",
            Description = $"Final exam for {randomEvlName}",
            Evls = [randomEvl]
        };
        
        examService.CreateCourseComponent(exam);
        
        return randomEvl;
    }

    private static string GetRandomCourseName()
    {
        var courseNames = new []
        {
            "I-OOSE",
            "Programming",
            "UX Design",
            "Web Development",
            "Requirements Engineering",
            "Software Testing",
            "Software Architecture",
            "Software Project Management",
            "Software Quality Assurance",
            "Software Development",
            "Software Engineering",
            "Software Maintenance",
            "Software Design",
            "Software Analysis",
            "Software Development Methodologies",
            "Software Development Processes",
            "Software Development Tools",
            "Software Development Techniques",
            "Software Development Principles",
            "Software Development Patterns",
            "Software Development Practices",
            "Software Development Strategies",
            "Software Development Concepts",
            "Software Development Technologies",
            "Software Development Frameworks",
            "Software Development Languages",
            "Software Development Paradigms",
            "Software Development Environments",
            "Software Development Platforms",
            "Software Development Models",
            "Software Development Approaches",
            "Software Development Strategies",
            "Software Development Techniques",
            "Software Development Tools",
            "Software Development Practices",
            "Software Development Patterns",
        };
        
        var randomIndex = new Random().Next(0, courseNames.Length);
        return courseNames[randomIndex];
    }

    public static string GetRandomEvlName()
    {
        var evlNames = new []
        {
            "I-OOSE",
            "Programming",
            "UX Design",
            "Web Development",
            "Requirements Engineering",
            "Software Testing",
            "Software Architecture",
            "Software Project Management",
            "Software Quality Assurance",
            "Software Development",
            "Software Engineering",
            "Software Maintenance",
            "Software Design",
            "Software Analysis",
        };
        
        var randomIndex = new Random().Next(0, evlNames.Length);
        return evlNames[randomIndex];
    }
}