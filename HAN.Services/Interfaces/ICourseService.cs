using HAN.Services.DTOs;

namespace HAN.Services;

public interface ICourseService
{
    // Create
    CourseDto CreateCourse(CourseDto course);
    CourseDto GetCourseById(int id);
    IEnumerable<EvlDto> GetEvls(int courseId);
    
    // Assign
    void AddEvlToCourse(int courseId, int evlId);
}