using HAN.Services;
using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;

namespace HAN.Client.Data;

public static class DataSeeder
{
    public static void SeedCourseData(IServiceProvider serviceProvider)
    {
        int iterations = 10;
        
        for (int i = 0; i < iterations; i++)
        {
            SeedCourseIteration(i, serviceProvider);
        }
    }

    private static void SeedCourseIteration(int iteration, IServiceProvider serviceProvider)
    {
        var evlService = serviceProvider.GetRequiredService<IEvlService>();
        var courseService = serviceProvider.GetRequiredService<ICourseService>();
        var lessonService = serviceProvider.GetRequiredService<LessonService>();
        var examService = serviceProvider.GetRequiredService<ExamService>();

        var evlDto1 = new EvlDto()
        {
            Name = $"OOP {iteration}",
            Description = "Object Oriented Programming",
        };
        var evlDto2 = new EvlDto()
        {
            Name = $"Databases {iteration}",
            Description = "Database Management",
        };

        evlDto1 = evlService.CreateEvl(evlDto1);
        evlDto2 = evlService.CreateEvl(evlDto2);
        
        var lesson = new LessonDto()
        {
            Name = $"Lesson {iteration}",
            Description = $"Introduction to Lesson {iteration}. After this lesson, you will be able to understand the basics of Lesson {iteration}.",
            Evls = [evlDto1, evlDto2]
        };
        
        var exam = new ExamDto()
        {
            Name = $"Exam {iteration}",
            Description = $"Introduction to Exam {iteration}. After this exam, you will be able to understand the basics of Exam {iteration}.",
            Evls = [evlDto1, evlDto2]
        };

        lessonService.CreateCourseComponent(lesson);
        examService.CreateCourseComponent(exam);
    
        var courseName =  $"{GetRandomCourseName()} {iteration}";
        
        var course = new CourseDto()
        {
            Name = courseName,
            Description = $"Introduction to {courseName}. After this course, you will be able to understand the basics of {courseName}.",
            Evls = [evlDto1, evlDto2]
        };

        courseService.CreateCourse(course);
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
}