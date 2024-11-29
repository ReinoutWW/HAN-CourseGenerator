using HAN.Data;
using HAN.Data.Entities;

namespace HAN.Tests;

public static class TestDbSeeder
{
    public static void SeedUsers(AppDbContext context, int seedUserCount, string userPrefix)
    {
        List<User> users = [];
        for (var i = 0; i < seedUserCount; i++)
        {
            users.Add(
                new User()
                {
                    Name = $"{userPrefix}{i+1}",
                    Email = $"{userPrefix}{i+1}@coursegenerator.com",
                    Password = "password",
                }
            );
        }
        
        context.Users.AddRange(users);
        context.SaveChanges();
    }

    public static void SeedCourses(AppDbContext context, int seedCourseCount)
    {
        List<Course> courses = [];
        for (var i = 0; i < seedCourseCount; i++)
        {
            courses.Add(
                new Course()
                {
                    Name = $"GeneratedCourse{i+1}",
                    Description = $"Description {i+1}",
                }
            );
        }
        
        context.Courses.AddRange(courses);
        context.SaveChanges();
    }
}