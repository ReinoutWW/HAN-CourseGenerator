using HAN.Services.DTOs;

namespace HAN.Services;

public interface IUserService
{
    public UserDto CreateUser(UserDto userDto);
    public UserDto? GetUserById(int id);
}