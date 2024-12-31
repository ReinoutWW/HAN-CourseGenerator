using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace HAN.Client.Authentication;

public class CustomAuthStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
{
    private const string TokenKey = "authToken"; // Key for token in localStorage

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsStringAsync(TokenKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            // User is not authenticated
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // Parse claims from token (if applicable)
        var identity = new ClaimsIdentity(ParseClaimsFromToken(token), "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void MarkUserAsAuthenticated(string token)
    {
        localStorage.SetItemAsync(TokenKey, token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void MarkUserAsLoggedOut()
    {
        localStorage.RemoveItemAsync(TokenKey);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private IEnumerable<Claim> ParseClaimsFromToken(string jwt)
    {
        // Logic to parse claims from the token
        var payload = jwt.Split('.')[1];
        var jsonBytes = Convert.FromBase64String(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }
}