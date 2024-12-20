﻿using System.ComponentModel.Design;
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

    public static void SeedLessons(AppDbContext context, int seedCourseComponentCount, int seedFileCount = 2)
    {
        var lessons = Enumerable.Range(0, seedCourseComponentCount)
            .Select(i => new CourseComponentBuilder()
                .AsLesson()
                .WithName($"Lesson {i}")
                .AddFiles(seedFileCount)
                .Build())
            .ToList();

        context.CourseComponents.AddRange(lessons);
        context.SaveChanges();
    }
    
    public static void SeedLessonsWithEvls(AppDbContext context, int seedCourseComponentCount, int seedEvlCount = 2)
    {
        var lessons = Enumerable.Range(0, seedCourseComponentCount)
            .Select(i => new CourseComponentBuilder()
                .AsLesson()
                .WithName($"Lesson {i}")
                .AddEvls(seedEvlCount)
                .Build())
            .ToList();

        context.CourseComponents.AddRange(lessons);
        context.SaveChanges();
    }
}