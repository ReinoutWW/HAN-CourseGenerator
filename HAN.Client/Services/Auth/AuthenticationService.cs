using System.Text.Json.Serialization;

namespace HAN.Client.Services.Auth;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenStorageService _tokenStorage;
    private readonly IConfiguration _configuration;
    
    private readonly string _apiScope;
    private readonly Uri _tokenEndpoint;

    public AuthenticationService(HttpClient httpClient, ITokenStorageService tokenStorage, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _tokenStorage = tokenStorage;
        _configuration = configuration;
        
        _tokenEndpoint = new Uri(_configuration["IdentityProvider:TokenEndpoint"] ?? throw new ArgumentNullException(nameof(configuration), "TokenEndpoint is not configured."));
        _apiScope = _configuration["IdentityProvider:ApiScope"] ?? throw new ArgumentNullException(nameof(configuration), "ApiScope is not configured.");
    }

    public async Task<AuthenticationResult> LoginAsync(string username, string password)
    {
        // Prepare the token request
        var tokenRequest = new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", "client" }, 
            { "client_secret", "secret" }, 
            { "scope", _apiScope },
            { "username", username },
            { "password", password }
        };

        var requestContent = new FormUrlEncodedContent(tokenRequest);
        var failedResult = new AuthenticationResult(false, "Unable to login, please check your credentials.");

        try
        {
            var response = await _httpClient.PostAsync(_tokenEndpoint, requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                return failedResult;
            }
            
            var accessToken = DeserializeAccessToken(responseContent);

            if (string.IsNullOrEmpty(accessToken)) 
                return failedResult;
            
            await _tokenStorage.SetTokenAsync(accessToken);
            var successResult = new AuthenticationResult(true, "Login successful.");
                
            return successResult;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login failed: {ex.Message}");
            return failedResult;
        }
    }

    private static string DeserializeAccessToken(string responseContent)
    {
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        if(tokenResponse == null)
            throw new InvalidOperationException("Failed to deserialize token response.");
        
        return tokenResponse.AccessToken ;
    }

    public async Task LogoutAsync()
    {
        await _tokenStorage.RemoveTokenAsync();
    }
}