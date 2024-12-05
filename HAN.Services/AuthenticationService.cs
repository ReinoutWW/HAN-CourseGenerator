using HAN.Services.DTOs;

namespace HAN.Services;

public interface IAuthenticationService
{
    bool Authenticate(AuthenticateUserDto user);
}

public class AuthenticationService : IAuthenticationService
{
    public bool Authenticate(AuthenticateUserDto user)
    {
        throw new NotImplementedException();
    }
}