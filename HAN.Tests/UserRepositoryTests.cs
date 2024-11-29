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
    private const int SeedUserCount = 2;
    private const string UserPrefix = "AnonymousUser";
    
    public UserRepositoryTests() : base()
    {
        _userRepository = base.ServiceProvider.GetRequiredService<IUserRepository>();
        _context = base.ServiceProvider.GetRequiredService<AppDbContext>();

        TestDbSeeder.SeedUsers(_context, SeedUserCount, UserPrefix);
    }

    [Fact]
    public void ShouldGetAllUsers()
    {
        var users = _context.Users.ToList();
        Assert.NotEmpty(users);
        Assert.Equal(SeedUserCount, users.Count);
    }
    
    [Fact]
    public void GetUserById_ShouldReturnCorrectUser()
    {
        var userId = 1;
        var userName = $"{UserPrefix}{userId}";
        
        // Act
        var user = _userRepository.GetUserById(userId);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(userName, user.Name);
    }

    [Fact]
    public void CreateUser_ShouldAddNewUser()
    {
        // Arrange
        var newUser = new User() { Name = $"{UserPrefix}{Guid.NewGuid()}" };

        // Act
        var createdUser = _userRepository.CreateUser(newUser);
        _userRepository.SaveChanges();
        var users = _userRepository.GetAllUsers().ToList();

        // Assert
        Assert.Contains(users, u => u.Name == createdUser.Name);
    }

    [Fact]
    public void CreateUser_ShouldThrowException_WhenUserAlreadyExists()
    {
        var user = new User() { Id = 1, Name = $"{UserPrefix}{Guid.NewGuid()}" };
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