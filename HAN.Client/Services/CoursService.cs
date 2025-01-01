using HAN.Client.Services.Models;

namespace HAN.Client.Services;

public class CourseService(ICourseApiAdapter adapter)
{
    public async Task<List<Course>> GetCoursesAsync()
    {
        return await adapter.FetchCoursesAsync();
    }
}