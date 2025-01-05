using System.Net;
using System.Text.Json;
using HAN.Client.Services.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace HAN.Client.Services;

public class ProfileApiAdapter(HttpClient httpClient, Uri baseUrl) : IProfileApiAdapter
{
    public async Task<Profile> GetUserProfile()
    {
        var url = new Uri(baseUrl, "/profile");
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await httpClient.GetAsync(url);

        if(response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException("User is not authorized to access this resource.");
        }
        
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to fetch user profile: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var profile = JsonSerializer.Deserialize<Profile>(content) ?? new Profile();
        return profile;
    }
}