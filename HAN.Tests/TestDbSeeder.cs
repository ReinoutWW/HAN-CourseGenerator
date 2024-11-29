using HAN.Data;
using HAN.Data.Entities;

namespace HAN.Tests;

public static class TestDbSeeder
{
    public static void SeedUsers(AppDbContext context)
    {
        context.Users.AddRange(
            new User { Name = "Alice" },
            new User { Name = "Bob" }
        );
        context.SaveChanges();
    }
}