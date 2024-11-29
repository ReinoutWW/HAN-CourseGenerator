using HAN.Data;
using HAN.Data.Entities;

namespace HAN.Tests;

public static class TestDbSeeder
{
    public static void SeedUsers(AppDbContext context)
    {
        context.Users.AddRange(
            new User() { Id = 1, Name = "Alice" },
            new User { Id = 2, Name = "Bob" }
        );
        context.SaveChanges();
    }
}