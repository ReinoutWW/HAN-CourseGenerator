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
        var fileService = serviceProvider.GetRequiredService<IFileService>();

        var randomEvlName = GetRandomEvlName();
        
        var randomEvl = new EvlDto()
        {
            Name = $"{randomEvlName}",
            Description = $"Learning everything about {randomEvlName}",
        };
        randomEvl = evlService.CreateEvl(randomEvl);
        
        var randomFile1 = fileService.CreateFile(GetRandomFile());
        var randomFile2 = fileService.CreateFile(GetRandomFile());
        var randomFile3 = fileService.CreateFile(GetRandomFile());
        var randomFile4 = fileService.CreateFile(GetRandomFile());
        var randomFile5 = fileService.CreateFile(GetRandomFile());
        var randomFile6 = fileService.CreateFile(GetRandomFile());

        var lessons = new List<LessonDto>()
        {
            new LessonDto()
            {
                Name = $"Learning the basics",
                Description = $"Introduction lesson to {randomEvlName}",
                Evls = [randomEvl],
                Files = [randomFile1, randomFile2]
            },
            new LessonDto()
            {
                Name = $"Learning the advanced",
                Description = $"Introduction to advanced {randomEvlName}",
                Evls = [randomEvl],
                Files = [randomFile3, randomFile4]
            }
        };

        lessons.ForEach(lesson => lessonService.CreateCourseComponent(lesson));

        var exam = new ExamDto()
        {
            Name = $"Final exam {randomEvlName}",
            Description = $"Final exam for {randomEvlName}",
            Evls = [randomEvl],
            Files = [randomFile5, randomFile6]
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
    
    public static string GetRandomFileName()
    {
        var fileNames = new []
        {
            "foundations-of-design", 
            "code-craft", 
            "user-journeys", 
            "building-blocks", 
            "blueprint-guide", 
            "testing-ground", 
            "system-structures", 
            "project-playbook", 
            "quality-matrix", 
            "dev-chronicles", 
            "engineered-solutions", 
            "sustainability-guide", 
            "creative-minds", 
            "insight-analysis", 
            "innovation-lab", 
            "process-mastery", 
            "logic-flows", 
            "architecture-vision", 
            "design-patterns", 
            "execution-strategies", 
            "performance-insights", 
            "maintenance-hub", 
            "future-thinking", 
            "workflow-harmony", 
            "analysis-spectrum",
            "concept-lab", 
            "code-reflections", 
            "experience-mapping", 
            "data-blueprint", 
            "problem-solver", 
            "system-dynamics", 
            "process-hub", 
            "visionary-ideas", 
            "quality-beacon", 
            "growth-pathways", 
            "knowledge-grid", 
            "innovation-canvas", 
            "logic-framework", 
            "collaboration-core", 
            "future-designs", 
            "debug-journal", 
            "architecture-essence", 
            "design-horizons", 
            "workflow-dynamics", 
            "analysis-journey"
        };
        
        var randomIndex = new Random().Next(0, fileNames.Length);
        return fileNames[randomIndex];
    }
    
    public static string GetRandomFileContent()
    {
        var fileContents = new[]
        {
            "Explore the key principles of effective design and their applications across various fields.",
            "Master the art of writing clean, efficient, and maintainable code.",
            "Understand the pathways users take and how to optimize their experience.",
            "Discover the fundamental components needed to construct robust solutions.",
            "A practical guide to creating blueprints for scalable and sustainable projects.",
            "Learn the essentials of creating a robust testing environment to ensure quality.",
            "Delve into the core principles of designing efficient system architectures.",
            "A step-by-step guide to managing successful projects from start to finish.",
            "Explore frameworks for assessing and improving quality in all aspects of your work.",
            "A collection of insights and best practices from seasoned developers.",
            "Learn how to craft tailored solutions to meet complex challenges.",
            "Guidelines for creating systems that are both efficient and environmentally friendly.",
            "Unlock your creativity with tools and techniques that inspire innovation.",
            "Analyze data to uncover meaningful insights and inform decision-making.",
            "A space to experiment with cutting-edge ideas and transform them into reality.",
            "Master efficient workflows to optimize time and resources.",
            "Learn how to design logical workflows that enhance clarity and productivity.",
            "Build a visionary framework for modern architectural challenges.",
            "Explore common design patterns and their real-world applications.",
            "Strategies to ensure the flawless execution of your projects.",
            "Gain a deeper understanding of system performance and improvement methods.",
            "Best practices for maintaining systems and ensuring long-term reliability.",
            "Foster a mindset that anticipates future challenges and drives innovation.",
            "Optimize your workflows with harmonious integration of tools and processes.",
            "Follow the journey of analysis to discover key opportunities for improvement.",
            "Experiment with conceptual ideas that drive innovation and creativity.",
            "Reflect on code practices to continuously improve your development skills.",
            "Map out experiences to enhance the journey of users through your designs.",
            "Create data-driven plans to address system architecture and development.",
            "Solve complex problems with a systematic approach and tested strategies.",
            "Dive into the dynamics of systems for better efficiency and effectiveness.",
            "Centralize your processes for smoother and faster operations.",
            "Capture visionary ideas that drive impactful and meaningful projects.",
            "Light the way with quality-focused strategies and performance goals.",
            "Discover growth pathways that lead to personal and professional development.",
            "Organize and connect knowledge to create a network of powerful insights.",
            "Sketch innovative ideas that lead to new opportunities and solutions.",
            "Frameworks that bring logical flow and harmony to your design processes.",
            "Coordinate collaboration efforts to achieve seamless teamwork outcomes.",
            "Draft sustainable plans for addressing the challenges of future generations.",
            "Document debugging methods that help uncover and resolve issues efficiently.",
            "Outline the essence of architectural principles for innovative systems.",
            "Discover new horizons for creative and impactful design solutions.",
            "Navigate the dynamics of workflows to improve performance and clarity.",
            "Embark on an analytical journey to uncover hidden patterns and solutions."
        };
        
        var randomIndex = new Random().Next(0, fileContents.Length);
        return fileContents[randomIndex];
    }
    
    public static FileDto GetRandomFile()
    {
        return new FileDto()
        {
            Name = GetRandomFileName(),
            Content = GetRandomFileContent()
        };
    }
    
}