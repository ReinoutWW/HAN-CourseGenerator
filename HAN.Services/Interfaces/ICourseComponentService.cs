using HAN.Services.DTOs;

namespace HAN.Services;

public interface ICourseComponentService
{
    // Create
    CourseComponentResponseDto CreateCourseComponent(CreateCourseComponentDto courseComponent);
    CourseComponentResponseDto GetCourseComponentById(int id);
    IEnumerable<EvlResponseDto> GetEvls(int courseComponentId);
    
    // Assign
    void AddEvlToCourseComponent(int courseComponentId, int evlId);
}