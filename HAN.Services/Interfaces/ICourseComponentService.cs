using HAN.Services.DTOs;

namespace HAN.Services;

public interface ICourseComponentService
{
    // Create
    CourseComponentDto CreateCourseComponent(CourseComponentDto courseComponent);
    CourseComponentDto GetCourseComponentById(int id);
    IEnumerable<EvlDto> GetEvls(int courseComponentId);
    
    // Assign
    void AddEvlToCourseComponent(int courseComponentId, int evlId);
}