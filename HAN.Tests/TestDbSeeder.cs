using HAN.Data;
using HAN.Data.Entities;

namespace HAN.Tests;

public static class TestDbSeeder
{
    public static void SeedUsers(AppDbContext context, int seedUserCount, string userPrefix)
    {
        List<User> users = new();
        for (int i = 0; i < seedUserCount; i++)
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
}