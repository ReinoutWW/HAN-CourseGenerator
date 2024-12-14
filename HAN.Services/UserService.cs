using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Validation;

namespace HAN.Services;

public interface IUserService
{
    public UserResponseDto CreateUser(CreateUserDto userDto);
    public UserResponseDto? GetUserById(int id);
}

public class UserService(IUserRepository repository, IMapper mapper, IValidationService validationService) : IUserService
{
    public UserResponseDto CreateUser(CreateUserDto userDto)
    {
        var userEntity = mapper.Map<User>(userDto);
    
        validationService.Validate(userEntity);
        repository.Add(userEntity);
    
        return mapper.Map<UserResponseDto>(userEntity);
    }

    public UserResponseDto? GetUserById(int id)
    {
        var user = repository.GetById(id);
        return mapper.Map<UserResponseDto>(user);
    }
}