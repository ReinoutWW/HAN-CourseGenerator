using HAN.Services.DTOs;

namespace HAN.Services;

public interface IUserService
{
    public UserResponseDto CreateUser(CreateUserDto userDto);
    public UserResponseDto? GetUserById(int id);
}