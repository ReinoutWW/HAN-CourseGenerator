namespace HAN.Client.Services.Auth;

public interface ITokenStorageService
{
    Task SetTokenAsync(string token);
    Task<string?> GetTokenAsync();
    Task RemoveTokenAsync();
}