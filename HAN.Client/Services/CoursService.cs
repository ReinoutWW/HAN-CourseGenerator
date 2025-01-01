using HAN.Client.Services.Models;

namespace HAN.Client.Services;

public class CourseService(ICourseApiAdapter adapter)
{
    public async Task<List<Course>> GetCoursesAsync()
    {
        try
        {
            return await adapter.FetchCoursesAsync();
        } catch (HttpRequestException e)
        {
            throw new Exception("Failed to fetch courses. Please try again later.");
        }
    }
}