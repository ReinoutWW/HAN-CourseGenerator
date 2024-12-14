using System.ComponentModel.Design;
using HAN.Data;
using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using Microsoft.EntityFrameworkCore;

namespace HAN.Tests.Base;

public static class TestDbSeeder
{
    public static void SeedUsers(AppDbContext context, int seedUserCount, string userPrefix)
    {
        var users = Enumerable.Range(0, seedUserCount)
            .Select(i => {
                var user = DbEntityCreator<User>.CreateEntity();
                user.Name = $"{userPrefix}{i + 1}";
                user.Email = $"{userPrefix}{i + 1}@coursegenerator.com";
                return user;
            })
            .ToList();

        context.Users.AddRange(users);
        context.SaveChanges();
    }

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
    
    private static List<Evl> SeedEvlsWithCourseComponents(AppDbContext context, int seedEvlCount)
    {
        var evls = SeedEvls(context, seedEvlCount);
        
        evls.SeedCourseComponentsForEvls(context);

        context.SaveChanges();

        return evls;
    }
    

    private static void SeedCourseComponentsForEvls(this List<Evl> evls, AppDbContext context)
    {
        evls.ForEach(evl =>
        {
            var courseComponent = DbEntityCreator<CourseComponent>.CreateEntity();

            context.CourseComponents.Add(courseComponent);
            context.SaveChanges();

            courseComponent = context.CourseComponents
                .Include(c => c.Evls)
                .Single(c => c.Id == courseComponent.Id);

            courseComponent.Evls.Add(evl);
        });
        
        context.SaveChanges();
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