namespace HAN.Client.Services.Auth;

public interface IAuthenticationService
{
    Task<AuthenticationResult> LoginAsync(string username, string password);
    Task LogoutAsync();
}