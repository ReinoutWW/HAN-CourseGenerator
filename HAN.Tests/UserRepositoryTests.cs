using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests;

public class UserRepositoryTests : TestBase, IDisposable
{
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;
    
    public UserRepositoryTests() : base()
    {
        _userRepository = base.ServiceProvider.GetRequiredService<IUserRepository>();
        _context = base.ServiceProvider.GetRequiredService<AppDbContext>();

        TestDbSeeder.SeedUsers(_context);
    }
    
    [Fact]
    public void GetUserById_ShouldReturnCorrectUser()
    {
        // Act
        var user = _userRepository.GetUserById(1);

        // Assert
        Assert.NotNull(user);
        Assert.Equal("Alice", user.Name);
    }

    [Fact]
    public void CreateUser_ShouldAddNewUser()
    {
        // Arrange
        var newUser = new User() { Name = "Charlie"};

        // Act
        var createdUser = _userRepository.CreateUser(newUser);
        _userRepository.SaveChanges();
        var users = _userRepository.GetAllUsers().ToList();

        // Assert
        Assert.Equal(3, users.Count);
        Assert.Contains(users, u => u.Name == createdUser.Name);
    }

    [Fact]
    public void CreateUser_ShouldThrowException_WhenUserAlreadyExists()
    {
        var user = new User() { Id = 1, Name = "Alice" };
        Assert.ThrowsAny<Exception>(() => _userRepository.CreateUser(user));        
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}