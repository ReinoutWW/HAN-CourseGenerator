using System.Diagnostics;
using HAN.Client.Services.Http;
using HAN.Client.Services.Models;

namespace HAN.Client.Services;

public class CourseService(ICourseApiAdapter adapter)
{
    public async Task<List<Course>> GetCoursesAsync()
    {
        try
        {
            return await adapter.FetchCoursesAsync();
        } catch (UnreachableException e)
        {
            throw new UnreachableException("Failed to fetch courses. Please try again later.");
        }
    }
}