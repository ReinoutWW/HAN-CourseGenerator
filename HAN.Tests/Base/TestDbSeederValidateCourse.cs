using HAN.Data;
using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using Microsoft.EntityFrameworkCore;

namespace HAN.Tests.Base;

public static class TestDbSeederValidateCourse
{
    public static void SeedCourses(AppDbContext context, int seedCourseCount, int seedValidEvlCount = 2)
    {
        var courses = Enumerable.Range(0, seedCourseCount)
            .Select(i => new CourseBuilder()
                .WithName($"Course {i}")
                .AddValidEvls(seedValidEvlCount) // Example: Adds 3 valid EVLs
                .Build())
            .ToList();

        context.Courses.AddRange(courses);
        context.SaveChanges();
    }

    public static int SeedValidCourseForValidation(AppDbContext context)
    {
        const int validEvlCount = 2;
        
        var course = new CourseBuilder()
            .WithName("Valid Course")
            .AddValidEvls(validEvlCount) // Adds 2 valid EVLs
            .Build();

        context.Courses.Add(course);
        context.SaveChanges();

        return course.Id;
    }

    public static int SeedInvalidLessonCourseForValidation(AppDbContext context)
    {
        const int invalidEvlCount = 0;
        
        var course = new CourseBuilder()
            .WithName("Invalid Course")
            .AddInvalidEvls(invalidEvlCount) // Adds 2 invalid EVLs
            .Build();

        context.Courses.Add(course);
        context.SaveChanges();

        return course.Id;
    }
}