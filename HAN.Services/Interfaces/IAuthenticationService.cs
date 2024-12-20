using HAN.Services.DTOs;

namespace HAN.Services.Interfaces;

public interface IAuthenticationService
{
    bool Authenticate(AuthenticateUserDto user);
}