namespace HAN.Client.Services.Auth;

public class AuthenticationResult(bool isSuccess, string? errorMessage)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public string? ErrorMessage { get; set; } = errorMessage;
}