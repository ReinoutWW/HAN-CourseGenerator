using System.Text.Json;
using System.Text.Json.Serialization;

namespace HAN.Client.API.RabbitMQ;

public class AuthService(HttpClient httpClient)
{
    public async Task<string> LoginAsync(string username, string password)
    {
        const string identityServerEndpoint = "https://localhost:7054/connect/token";
        
        // Prepare the token request
        var tokenRequest = new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", "client" }, 
            { "client_secret", "secret" }, 
            { "scope", "courseapi" },
            { "username", username },
            { "password", password }
        };

        var requestContent = new FormUrlEncodedContent(tokenRequest);

        try
        {
            var response = await httpClient.PostAsync(identityServerEndpoint, requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }
            
           return DeserializeAccessToken(responseContent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login failed: {ex.Message}");
            throw new Exception($"Failed to retrieve access token. {ex.Message}");
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
    
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;
    }
}