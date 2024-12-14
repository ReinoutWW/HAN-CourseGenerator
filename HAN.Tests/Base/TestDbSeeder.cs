using System.ComponentModel.Design;
using HAN.Data;
using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using Microsoft.EntityFrameworkCore;

namespace HAN.Tests.Base;

public static class TestDbSeeder
{
    public static void SeedCourses(AppDbContext context, int seedCourseCount)
    {
        var courses = Enumerable.Range(0, seedCourseCount)
            .Select(_ => DbEntityCreator<Course>.CreateEntity())
            .ToList();

        context.Courses.AddRange(courses);
        context.SaveChanges();
    }

    public static List<Evl> SeedEvls(AppDbContext context, int seedEvlCount)
    {
        var evls = Enumerable.Range(0, seedEvlCount)
            .Select(_ => DbEntityCreator<Evl>.CreateEntity())
            .ToList();

        context.Evls.AddRange(evls);
        context.SaveChanges();

        return evls;
    }
    
    public static void SeedCourseComponents(AppDbContext context, int seedCourseComponentCount)
    {
        var courseComponents = Enumerable.Range(0, seedCourseComponentCount)
            .Select(_ => DbEntityCreator<CourseComponent>.CreateEntity())
            .ToList();

        context.CourseComponents.AddRange(courseComponents);
        context.SaveChanges();
    }
}