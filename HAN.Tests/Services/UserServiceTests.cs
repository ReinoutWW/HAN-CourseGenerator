using HAN.Services;
using HAN.Services.DTOs;
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

        var createdCourse = _userService.CreateUser(user);
        
        Assert.NotNull(createdCourse);
        Assert.Equal(user.Name, createdCourse.Name);
        Assert.Equal(user.Email, createdCourse.Email);
    }
}