using HAN.Services.DTOs;

namespace HAN.Services.Interfaces;

public interface IUserService
{
    public UserDto CreateUser(UserDto userDto);
    public UserDto? GetUserById(int id);
}