using HAN.Services;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Tests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Tests.Services;

public class UserServiceTests : TestBase
{
    private readonly IUserService _userService;
        
    public UserServiceTests()
    {
        _userService = ServiceProvider.GetRequiredService<IUserService>();
    }

    [Fact]
    public void CreateUser_ShouldCreateUser()
    {
        UserDto user = new()
        {
            Name = "Test Course",
            Email = "test@test.com",
            Password = "password"
        };

        var createdUser = _userService.CreateUser(user);
        
        Assert.NotNull(createdUser);
        Assert.Equal(user.Name, createdUser.Name);
        Assert.Equal(user.Email, createdUser.Email);
    }
    
    [Fact]
    public void GetUserById_ShouldReturnUser()
    {
        UserDto user = new()
        {
            Name = "Test Course",
            Email = "test@test.com",
            Password = "password"
        };

        var createdUser = _userService.CreateUser(user);
        var getUser = _userService.GetUserById(createdUser.Id);
        
        Assert.NotNull(getUser);
        Assert.Equal(user.Name, getUser.Name);
        Assert.Equal(user.Email, getUser.Email);
    }
}