﻿using HAN.Data;
using HAN.Data.Entities;

namespace HAN.Tests.Base;

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
    
    public static void SeedEvls(AppDbContext context, int seedEvlCount)
    {
        List<Evl> evls = [];
        for (var i = 0; i < seedEvlCount; i++)
        {
            evls.Add(
                new Evl()
                {
                    Name = $"GeneratedEvl{i+1}",
                    Description = $"Description {i+1}",
                }
            );
        }
        
        context.Evls.AddRange(evls);
        context.SaveChanges();
    }
}