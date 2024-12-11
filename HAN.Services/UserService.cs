using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Validation;

namespace HAN.Services;

public interface IUserService
{
    public UserResponseDto CreateUser(CreateUserDto userDto);
    public UserResponseDto? GetUserById(string id);
}

public class UserService(IUserRepository repository, IMapper mapper, IValidationService validationService) : IUserService
{
    public UserResponseDto CreateUser(CreateUserDto userDto)
    {
        var userDomainEntity = mapper.Map<User>(userDto);
    
        validationService.Validate(userDomainEntity);
    
        var userResult = repository.CreateUser(
            mapper.Map<Data.Entities.User>(userDto)
        );
        repository.SaveChanges();
    
        return mapper.Map<UserResponseDto>(userResult);
    }

    public UserResponseDto? GetUserById(string id)
    {
        throw new NotImplementedException();
    }
}