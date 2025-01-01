using HAN.Client.Services.Models;

namespace HAN.Client.Services;

public interface ICourseApiAdapter
{
    Task<List<Course>> FetchCoursesAsync();
}