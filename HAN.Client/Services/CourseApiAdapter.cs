using System.Text.Json;
using HAN.Client.Services.Models;

namespace HAN.Client.Services;

public class CourseApiAdapter(HttpClient httpClient, Uri baseUrl) : ICourseApiAdapter
{
    public async Task<List<Course>> FetchCoursesAsync()
    {
        var response = await httpClient.GetAsync(new Uri(baseUrl, "/course"));

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to fetch courses: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var courses = JsonSerializer.Deserialize<List<Course>>(content) ?? new List<Course>();

        return courses;
    }
}