using HAN.Services.DTOs;

namespace HAN.Console;

public static class DemoCourseLogger
{
    public static void LogEvl(EvlDto evl)
    {
        System.Console.WriteLine("");
        System.Console.WriteLine($"Evl {evl.Id} ");
        System.Console.WriteLine($"- {evl.Name}");
        System.Console.WriteLine($"- {evl.Description}");
    }

    public static void LogCourse(CourseDto createCourseDto)
    {
        System.Console.WriteLine("-------- Course: -----");
        System.Console.WriteLine(createCourseDto.Id);
        System.Console.WriteLine(createCourseDto.Name);
        System.Console.WriteLine(createCourseDto.Description);
        System.Console.WriteLine("-------------");
    }
}