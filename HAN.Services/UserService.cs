using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Services.Validation;

namespace HAN.Services;

public class UserService(IUserRepository repository, IMapper mapper, IValidationService validationService) : IUserService {
    public UserDto CreateUser(UserDto userDto)
    {
        validationService.Validate(userDto);
        
        var userEntity = mapper.Map<User>(userDto);
        repository.Add(userEntity);
    
        return mapper.Map<UserDto>(userEntity);
    }

    public UserDto? GetUserById(int id)
    {
        var user = repository.GetById(id);
        return mapper.Map<UserDto>(user);
    }
}