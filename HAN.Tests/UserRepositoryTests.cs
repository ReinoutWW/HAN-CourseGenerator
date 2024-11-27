using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HAN.Tests;

public class UserRepositoryTests
{
    private static AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        
        return new AppDbContext(options);
    }
    
    private void SeedDatabase(AppDbContext context)
    {
        var repository = new UserRepository(context);
        
        if (repository.GetAllUsers().Any())
        {
            return;
        }
        
        var users = new List<User>
        {
            new User { Id = 1, Name = "John Doe" },
            new User { Id = 2, Name = "Jane Doe" }
        };
        
        context.Users.AddRange(users);
        context.SaveChanges();
    }
    
     [Fact]
    public void GetAllUsers_ShouldReturnAllUsers()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        SeedDatabase(context);
        var repository = new UserRepository(context);

        // Act
        var result = repository.GetAllUsers();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetUserById_ShouldReturnUser_WhenIdExists()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        SeedDatabase(context);
        var repository = new UserRepository(context);

        // Act
        var result = repository.GetUserById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John Doe", result.Name);
    }

    [Fact]
    public void GetUserById_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        SeedDatabase(context);
        var repository = new UserRepository(context);
        Exception? thrownException = null;

        // Act
        try
        {
            var result = repository.GetUserById(99);
        }
        catch (Exception exception)
        {
            thrownException = exception;
        }
        
        Assert.NotNull(thrownException);
        Assert.IsType<KeyNotFoundException>(thrownException);
    }

    [Fact]
    public void CreateUser_ShouldAddUserToDatabase()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new UserRepository(context);
        var newUser = new User { Id = 3, Name = "New User"};

        // Act
        repository.CreateUser(newUser);
        repository.SaveChanges();
        
        var result = repository.GetUserById(3);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New User", result.Name);
    }

    [Fact]
    public void SaveChanges_ShouldPersistChanges()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new UserRepository(context);
        var newUser = new User { Id = 4, Name = "New User"};

        // Act
        repository.CreateUser(newUser);
        var saveResult = repository.SaveChanges();
        var result = repository.GetUserById(4);

        // Assert
        Assert.True(saveResult);
        Assert.NotNull(result);
    }
}