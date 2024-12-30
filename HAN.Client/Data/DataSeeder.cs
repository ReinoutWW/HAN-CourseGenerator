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
        var courseService = serviceProvider.GetRequiredService<ICourseService>();

        var oopEvl = SeedOopEvl(serviceProvider);
        var databaseEvl = SeedDatabaseEvl(serviceProvider);
        
        var courseName = GetRandomCourseName();
        var course = new CourseDto()
        {
            Name = courseName,
            Description = $"({iteration}): Introduction to {courseName}. After this course, you will be able to understand the basics of {courseName}.",
            Evls = [oopEvl, databaseEvl]
        };

        courseService.CreateCourse(course);
    }

    private static EvlDto SeedDatabaseEvl(IServiceProvider serviceProvider)
    {
        var evlService = serviceProvider.GetRequiredService<IEvlService>();
        var lessonService = serviceProvider.GetRequiredService<LessonService>();
        var examService = serviceProvider.GetRequiredService<ExamService>();
        
        var databaseEvl = new EvlDto()
        {
            Name = $"Database",
            Description = "Database management",
        };
        
        databaseEvl = evlService.CreateEvl(databaseEvl);

        var lessons = new List<LessonDto>()
        {
            new LessonDto()
            {
                Name = $"Learning the basics",
                Description = $"Introduction lesson to databases",
                Evls = [databaseEvl]
            },
            new LessonDto()
            {
                Name = $"Learning the basics",
                Description = $"Introduction to advanced databases",
                Evls = [databaseEvl]
            }
        };

        lessons.ForEach(lesson => lessonService.CreateCourseComponent(lesson));

        var exam = new ExamDto()
        {
            Name = $"Final exam Database",
            Description = $"Final exam for Database",
            Evls = [databaseEvl]
        };
        
        examService.CreateCourseComponent(exam);
        
        return databaseEvl;
    }

    private static EvlDto SeedOopEvl(IServiceProvider serviceProvider)
    {
        var evlService = serviceProvider.GetRequiredService<IEvlService>();
        var lessonService = serviceProvider.GetRequiredService<LessonService>();
        var examService = serviceProvider.GetRequiredService<ExamService>();
        
        var oopEvl = new EvlDto()
        {
            Name = $"OOP",
            Description = "Object Oriented Programming",
        };
        
        oopEvl = evlService.CreateEvl(oopEvl);

        var lessons = new List<LessonDto>()
        {
            new LessonDto()
            {
                Name = $"Learning the basics",
                Description = $"Introduction lesson to OOP",
                Evls = [oopEvl]
            },
            new LessonDto()
            {
                Name = $"Learning the basics",
                Description = $"Introduction to advanced OOP",
                Evls = [oopEvl]
            }
        };

        lessons.ForEach(lesson => lessonService.CreateCourseComponent(lesson));

        var exam = new ExamDto()
        {
            Name = $"Final exam OOP",
            Description = $"Final exam for OOP",
            Evls = [oopEvl]
        };
        
        examService.CreateCourseComponent(exam);
        
        return oopEvl;
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