using HAN.Data;
using HAN.Data.Entities;

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

    public static void SeedEvls(AppDbContext context, int seedEvlCount)
    {
        var evls = Enumerable.Range(0, seedEvlCount)
            .Select(_ => DbEntityCreator<Evl>.CreateEntity())
            .ToList();

        context.Evls.AddRange(evls);
        context.SaveChanges();
    }
}