using HAN.Domain.Entities.CourseComponents;
using HAN.Services.DTOs;

namespace HAN.Services;

public interface ICourseService
{
    // Create
    CourseResponseDto CreateCourse(CreateCourseDto course);
    CourseComponent CreateCourseComponent(CourseComponent component);
    CourseResponseDto GetCourseById(int id);
    IEnumerable<EvlResponseDto> GetEvls(int courseId);
    
    // Assign
    void AddEVLToCourse(int courseId, int evlId);
}