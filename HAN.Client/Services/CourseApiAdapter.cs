using System.Diagnostics;
using System.Net;
using System.Text.Json;
using HAN.Client.Services.Http;
using HAN.Client.Services.Models;

namespace HAN.Client.Services;

public class CourseApiAdapter(HttpClient httpClient, Uri baseUrl) : ICourseApiAdapter
{
    public async Task<List<Course>> FetchCoursesAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, new Uri(baseUrl, "/course"));
        var response = await httpClient.SendAsync(request);

        if(response.StatusCode == HttpStatusCode.Unauthorized) 
            throw new NotAuthorizedException("You are not authorized to access this resource.");
        
        if (!response.IsSuccessStatusCode)
        {
            throw new UnreachableException($"Failed to fetch courses: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var courses = JsonSerializer.Deserialize<List<Course>>(content) ?? new List<Course>();

        return courses;
    }
}