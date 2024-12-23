using HAN.Services.DTOs;
using HAN.Services.Interfaces;

namespace HAN.Client.Data;

public static class DataSeeder
{
    public static void SeedCourseData(IServiceProvider serviceProvider)
    {
        var evlService = serviceProvider.GetRequiredService<IEvlService>();
        var courseService = serviceProvider.GetRequiredService<ICourseService>();

        var evlDto1 = new EvlDto()
        {
            Name = "OOP",
            Description = "Object Oriented Programming",
        };
        var evlDto2 = new EvlDto()
        {
            Name = "Databases",
            Description = "Database Management",
        };

        evlDto1 = evlService.CreateEvl(evlDto1);
        evlDto2 = evlService.CreateEvl(evlDto2);

        var course = new CourseDto()
        {
            Name = "I-OOSE",
            Description = "Introduction to Object Oriented Software Engineering",
            Evls = [evlDto1, evlDto2]
        };

        courseService.CreateCourse(course);
    }
}