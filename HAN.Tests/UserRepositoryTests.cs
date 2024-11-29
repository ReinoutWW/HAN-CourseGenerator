using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests;

public class UserRepositoryTests : TestBase
{
    private readonly IUserRepository _userRepository;
    private const int SeedUserCount = 2;
    private const string UserPrefix = "AnonymousUser";
    
    public UserRepositoryTests()
    {
        _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();

        TestDbSeeder.SeedUsers(Context, SeedUserCount, UserPrefix);
    }

    [Fact]
    public void ShouldGetAllUsers()
    {
        var getUsers = _userRepository.GetAllUsers().ToList();
        
        Assert.NotEmpty(getUsers);
        Assert.Equal(SeedUserCount, getUsers.Count);
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
    
    [Theory]
    [InlineData("UserName1")]
    [InlineData("UserName2")]
    public void CreateUser_ShouldAddNewUser(string userName)
    {
        var newUser = new User()
        {
            Name = userName,
            Email = $"{userName}@example.com",
            Password = "password"
        };

        // Act
        var createdUser = _userRepository.CreateUser(newUser);
        _userRepository.SaveChanges();
        var users = _userRepository.GetAllUsers().ToList();

        // Assert
        Assert.Contains(users, u => u.Name == createdUser.Name);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("PasswordIsWayTooLongBecauseSecurityExpectedUsToCreateAPasswordThatExceedsTheLimitedPasswordLength")]
    public void CreateUser_ShouldThrowException_PasswordIsNotCorrect(string password)
    {
        var newUser = new User()
        {
            Name = "username",
            Email = "username@example.com",
            Password = password
        };

        AddUserExpectValidationException(newUser);
    }


    [Theory]
    [InlineData("UserName11111111111111111111111111111111111111111111111111111111111111111111111111111UserName11111111111111111111111111111111111111111111111111111111111111111111111111111")]
    [InlineData("")]
    public void CreateUser_ShouldThrowException_WhenWrongUserName(string userName)
    {
        var newUser = new User()
        {
            Name = userName
        };
       
        AddUserExpectValidationException(newUser);
    }
    
  
    [Fact]
    public void CreateUser_ShouldThrowException_WhenUserAlreadyExists()
    {
        var user = new User() { Id = 1, Name = $"{UserPrefix}{Guid.NewGuid()}" };
        Assert.ThrowsAny<Exception>(() => _userRepository.CreateUser(user));        
    }

    [Fact]
    public void CreateUser_ShouldThrowException_WhenUserIsNull()
    {
        Exception? expectedException = Record.Exception(() =>
        {
            _userRepository.CreateUser(null!);
            _userRepository.SaveChanges();
        });

        Assert.NotNull(expectedException);
        Assert.IsType<ArgumentException>(expectedException);
    }
    
    private void AddUserExpectValidationException(User newUser)
    {
        Exception? expectedException = Record.Exception(() =>
        {
            _userRepository.CreateUser(newUser);
            _userRepository.SaveChanges();
        });

        Assert.NotNull(expectedException);
        Assert.IsType<AggregateException>(expectedException);
    }

}