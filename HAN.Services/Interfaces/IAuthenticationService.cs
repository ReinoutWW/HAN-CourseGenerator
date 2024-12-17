using HAN.Services.DTOs;

namespace HAN.Services;

public interface IAuthenticationService
{
    bool Authenticate(AuthenticateUserDto user);
}